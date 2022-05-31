using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders {
    [RequireComponent(typeof(SIBonusColliderBehaviour))]
    public class SIBonus : MonoBehaviour,  IPoolable {
        [SerializeField] GameObject _bonusRoot;
        [SerializeField] SIBonusAnimatorController _animatorController;
        [SerializeField] SIBonusMovement _bonusMovement;
        [SerializeField] SIBonusVariantSelector _variantSelector;
        
        bool _isAnimationTaskActive;
        Transform _thisTransform;
        CancellationTokenSource _cancellationTokenSource;
        
        public BonusSettings GetBonusVariantSettings() {
            return _variantSelector.BonusVariantSettings;
        }
        
        public void SetBonusVariant(BonusType bonusType) {
            _variantSelector.UseVariant(bonusType);
        }
        
        public void SetSpawnPosition(Vector3 spawnPos) {
            _bonusMovement.SetDropPosition(spawnPos);
        }

        public void SetSpawnRotation(Vector3 spawnRot) {
            //Intentionally unimplemented.
        }

        public void ManageScreenVisibility() {
            if (_thisTransform == null)
                _thisTransform = transform;
            
            if (_thisTransform && SIScreenUtils.IsInVerticalWorldScreenLimit(_thisTransform.position.y))
                return;
            
            _bonusMovement.StopObject();
        }

        public void PerformOnPoolActions() {
            //Note: This line resets the bonus before release.
            TryEnableBonusAndSelectedVariant(false);
            _bonusMovement.StopObject();
                        
            TryEnableBonusAndSelectedVariant(true);
            _bonusMovement.MoveObject();
        }

        public void TryRunBonusCollectedRoutine() {
            if (_isAnimationTaskActive)
                return;

            RefreshCancellation();
            StopBonusAnimationTask().Forget();
        }

        async UniTaskVoid StopBonusAnimationTask() {
            try {
                _isAnimationTaskActive = true;
                _animatorController.SetHideAnimation();
                while (_animatorController.IsVariantAnimationTriggered)
                    await WaitForUtils.SkipFramesTask(1, _cancellationTokenSource.Token);

                _isAnimationTaskActive = false;
                _bonusMovement.StopObject();
            }
            catch (OperationCanceledException) { } 
        }
        
        void RefreshCancellation() {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }
        
        void TryEnableBonusAndSelectedVariant(bool isEnabled) {
            
            _variantSelector.TryUpdateBonusVariant(isEnabled);
            _bonusRoot.SetActive(isEnabled);
            if(isEnabled)
                _animatorController.SetShowAnimation(_variantSelector.CurrentVariantRenderer);
        }
    }
}    