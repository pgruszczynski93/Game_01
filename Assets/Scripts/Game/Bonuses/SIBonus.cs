using System;
using UnityEngine;

namespace SpaceInvaders {
    [RequireComponent(typeof(SIBonusColliderBehaviour))]
    public class SIBonus : MonoBehaviour, ICanMove {
        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] protected GameObject _bonusRoot;

        [SerializeField] Transform _parent;
        [SerializeField] SIBonusAnimatorController _animatorController;
        [SerializeField] SIBonusDictionary _bonusesVariants;

        BonusType _bonusType;
        Vector3 _currentDropPos;
        
        BonusSettings _currentVariantSettings;
        Renderer _currentBonusVariantRenderer;
        Renderer _lastRenderer;
        Transform _thisTransform;
        
        public BonusSettings BonusVariantSettings => _currentVariantSettings;
        public Renderer BonusVariantRenderer => _currentBonusVariantRenderer;

        public Transform Parent {
            set => _parent = value;
            get => _parent;
        }

        void SetTransform() {
            if(_thisTransform == null)
                _thisTransform = transform;
        }

        void OnEnable() {
            SetTransform();
            SubscribeEvents();
        }

        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents() {
            SIEventsHandler.OnUpdate += TryToResetObject;
        }

        void UnsubscribeEvents() {
            SIEventsHandler.OnUpdate -= TryToResetObject;
        }

        public void SetAndReleaseBonusVariant(Vector3 releasePos, BonusType bonusType) {
            _currentDropPos = releasePos;
            _bonusType = bonusType;
            _currentVariantSettings = _bonusesVariants[_bonusType].scriptableBonus.bonusSettings;
            _currentBonusVariantRenderer = _bonusesVariants[_bonusType].bonusRenderer;
            
            //Note: This line ensures that bonus is reseted
            MoveObject();
        }

        public void MoveObject() {
            TryEnableBonus(true);
            ReleaseObject();
        }

        public void StopObject() {
            SetTransform();
            _rigidbody.velocity = SIHelpers.VectorZero;
            _thisTransform.SetParent(_parent);
            _thisTransform.localPosition = SIScreenUtils.HiddenObjectPosition;
            TryEnableBonus(false);
        }

        void TryEnableBonus(bool isEnabled) {
            
            if (_currentBonusVariantRenderer == _lastRenderer)
                return;
            
            if (_lastRenderer != null && _currentBonusVariantRenderer != _lastRenderer) {
                _animatorController.ResetAnimation();
                _lastRenderer.enabled = false;
            }
            
            _currentBonusVariantRenderer.enabled = isEnabled;
            _bonusRoot.SetActive(isEnabled);
            _lastRenderer = _currentBonusVariantRenderer;
            _animatorController.RunAnimation(_currentBonusVariantRenderer);
        }

        void TryToResetObject() {
             Vector3 bonusViewPortPosition =
                SIGameMasterBehaviour.Instance.MainCamera.WorldToViewportPoint(_thisTransform.position);

            if (!bonusViewPortPosition.IsInVerticalViewportSpace()) 
                StopObject();
        }

        void ReleaseObject() {
            _rigidbody.velocity = SIHelpers.VectorZero;
            _thisTransform.SetParent(null);
            _thisTransform.position = _currentDropPos;
            _rigidbody.AddForce(SIHelpers.VectorDown * _currentVariantSettings.releaseForceMultiplier,
                ForceMode.Impulse);
        }
    }
}