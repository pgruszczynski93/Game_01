using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PG.Game.Configs;
using PG.Game.Features.ObjectsPool;
using PG.Game.Helpers;
using PG.Game.Systems;
using UnityEngine;

namespace PG.Game.Bonuses {
    [RequireComponent(typeof(BonusColliderBehaviour))]
    public class BonusBehaviour : MonoBehaviour, IPoolable {
        [SerializeField] GameObject _bonusRoot;
        [SerializeField] BonusAnimatorController _animatorController;
        [SerializeField] BonusMovement _bonusMovement;
        [SerializeField] BonusVariantSelector _variantSelector;

        bool _isAnimationTaskActive;
        Transform _thisTransform;
        CancellationTokenSource _bonusCancellation;

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

            if (_thisTransform && ScreenUtils.IsInVerticalWorldScreenLimit(_thisTransform.position.y))
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
                    await WaitUtils.SkipFramesTask(1, _bonusCancellation.Token);

                _isAnimationTaskActive = false;
                _bonusMovement.StopObject();
            }
            catch (OperationCanceledException) { }
        }

        void RefreshCancellation() {
            _bonusCancellation?.Cancel();
            _bonusCancellation?.Dispose();
            _bonusCancellation = new CancellationTokenSource();
        }

        void TryEnableBonusAndSelectedVariant(bool isEnabled) {
            _variantSelector.TryUpdateBonusVariant(isEnabled);
            _bonusRoot.SetActive(isEnabled);
            if (isEnabled)
                _animatorController.SetShowAnimation(_variantSelector.CurrentVariantRenderer);
        }
    }
}