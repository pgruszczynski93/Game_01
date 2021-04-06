using System;
using System.Collections;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders {
    [RequireComponent(typeof(SIBonusColliderBehaviour))]
    public class SIBonus : MonoBehaviour, ICanMove, IPoolable {
        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] protected GameObject _bonusRoot;

        [SerializeField] Transform _parent;
        [SerializeField] SIBonusAnimatorController _animatorController;
        [SerializeField] SIBonusDictionary _bonusesVariants;
        [SerializeField] Transform _thisTransform;

        bool _isInStopRoutine;
        BonusType _bonusType;
        Vector3 _currentDropPos;

        Coroutine _stopCoroutine;
        BonusSettings _currentVariantSettings;
        Renderer _currentBonusVariantRenderer;
        Renderer _lastRenderer;
        
        public BonusSettings BonusVariantSettings => _currentVariantSettings;
        public Renderer BonusVariantRenderer => _currentBonusVariantRenderer;

        public Transform Parent {
            set => _parent = value;
            get => _parent;
        }

        void OnEnable() => SubscribeEvents();

        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents() {
            SIEventsHandler.OnUpdate += CheckIsInVerticalRange;
        }

        void UnsubscribeEvents() {
            SIEventsHandler.OnUpdate -= CheckIsInVerticalRange;
        }

        public void SetSpawnPosition(Vector3 spawnPos) {
            _currentDropPos = spawnPos;
        }

        public void SetBonusVariant(BonusType bonusType) {
            _bonusType = bonusType;
            _currentVariantSettings = _bonusesVariants[_bonusType].scriptableBonus.bonusSettings;
            _currentBonusVariantRenderer = _bonusesVariants[_bonusType].bonusRenderer;
        }
        
        public void UseObjectFromPool() {
            //Note: This line resets the bonus before release.
            StopObject();
            
            if(_stopCoroutine != null)
                StopCoroutine(_stopCoroutine);
            
            MoveObject();
        }

        public void MoveObject() {
            TryEnableBonusAndSelectedVariant(true);
            SetMotion();
        }

        public void StopObject() {
            TryEnableBonusAndSelectedVariant(false);
            ResetMotion();
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
            StopObject();
        }
        
        void SetMotion() {
            _rigidbody.velocity = SIHelpers.VectorZero;
            _thisTransform.SetParent(null);
            _thisTransform.position = _currentDropPos;
            _rigidbody.AddForce(SIHelpers.VectorDown * _currentVariantSettings.releaseForceMultiplier,
                ForceMode.Impulse);
        }

        void ResetMotion() {
            _rigidbody.velocity = SIHelpers.VectorZero;
            _thisTransform.SetParent(_parent);
            _thisTransform.localPosition = SIScreenUtils.HiddenObjectPosition;
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

        void CheckIsInVerticalRange() {
             Vector3 bonusViewPortPosition =
                SIGameMasterBehaviour.Instance.MainCamera.WorldToViewportPoint(_thisTransform.position);

            if (!bonusViewPortPosition.IsInVerticalViewportSpace()) 
                StopObject();
        }
    }
}    