using System.Collections;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders {
    [RequireComponent(typeof(SIBonusColliderBehaviour))]
    public class SIBonus : MonoBehaviour, IPoolable {
        [SerializeField] GameObject _bonusRoot;
        [SerializeField] SIBonusAnimatorController _animatorController;
        [SerializeField] SIBonusDictionary _bonusesVariants;
        [SerializeField] SIBonusMovement _bonusMovement;
        [SerializeField] SIBonusVariantSelector _variantSelector;
        
        bool _isInStopRoutine;
        BonusType _bonusType;

        BonusSettings _currentVariantSettings;
        Renderer _currentBonusVariantRenderer;
        Renderer _lastRenderer;
        Coroutine _stopCoroutine;
        
        public BonusSettings BonusVariantSettings => _currentVariantSettings;
        public Renderer BonusVariantRenderer => _currentBonusVariantRenderer;

        public void SetBonusVariant(BonusType bonusType) {
            _bonusType = bonusType;
            _currentVariantSettings = _bonusesVariants[_bonusType].scriptableBonus.bonusSettings;
            _currentBonusVariantRenderer = _bonusesVariants[_bonusType].bonusRenderer;
        }
        
        public void SetSpawnPosition(Vector3 spawnPos) {
            _bonusMovement.SetDropPosition(spawnPos);
        }

        public void SetSpawnRotation(Vector3 spawnRot) {
            //intentionally unimplemented
        }

        public void ManageScreenVisibility() {
            // to do: zarządzanie na ekranie
        }

        public void UseObjectFromPool() {
            //Note: This line resets the bonus before release.
            TryEnableBonusAndSelectedVariant(false);
            _bonusMovement.StopObject();
            
            if(_stopCoroutine != null)
                StopCoroutine(_stopCoroutine);
            
            TryEnableBonusAndSelectedVariant(true);
            _bonusMovement.MoveObject();
        }

        public void TryRunBonusCollectedRoutine() {
            if (_isInStopRoutine)
                return;
            
            _stopCoroutine = StartCoroutine(RunStopRoutine());
        }

        IEnumerator RunStopRoutine() {
            _isInStopRoutine = true;
            _animatorController.SetHideAnimation();
            while (_animatorController.IsVariantAnimationTriggered)
                yield return WaitUtils.SkipFrames(1);

            _isInStopRoutine = false;
            _bonusMovement.StopObject();
        }
        
        void TryEnableBonusAndSelectedVariant(bool isEnabled) {
            
            if (_currentBonusVariantRenderer == null)
                return;
            
            if (_lastRenderer != null) {
                _lastRenderer.enabled = false;
            }
            
            _bonusRoot.SetActive(isEnabled);
            _currentBonusVariantRenderer.enabled = isEnabled;
            _lastRenderer = _currentBonusVariantRenderer;
            
            if(isEnabled)
                _animatorController.SetShowAnimation(_currentBonusVariantRenderer);
        }
    }
}    