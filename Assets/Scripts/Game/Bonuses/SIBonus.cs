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
        [SerializeField] Transform _thisTransform;
        
        BonusType _bonusType;
        Vector3 _currentDropPos;
        
        BonusSettings _currentVariantSettings;
        [SerializeField] Renderer _currentBonusVariantRenderer;
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

        public void SetAndReleaseBonusVariant(Vector3 releasePos, BonusType bonusType) {
            //Note: This line resets the bonus before release.
            StopObject();
            
            _currentDropPos = releasePos;
            _bonusType = bonusType;
            _currentVariantSettings = _bonusesVariants[_bonusType].scriptableBonus.bonusSettings;
            _currentBonusVariantRenderer = _bonusesVariants[_bonusType].bonusRenderer;
            
            MoveObject();
        }

        public void MoveObject() {
            TryEnableBonus(true);
            SetMotion();
        }

        public void StopObject() {
            ResetMotion();
            TryEnableBonus(false);
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

        void TryEnableBonus(bool isEnabled) {
            
            if (_currentBonusVariantRenderer == null)
                return;
            
            if (_lastRenderer != null) {
                _lastRenderer.enabled = false;
            }
            
            _bonusRoot.SetActive(isEnabled);
            _currentBonusVariantRenderer.enabled = isEnabled;
            _animatorController.ReloadAnimation(_currentBonusVariantRenderer);
            _lastRenderer = _currentBonusVariantRenderer;
        }

        void CheckIsInVerticalRange() {
             Vector3 bonusViewPortPosition =
                SIGameMasterBehaviour.Instance.MainCamera.WorldToViewportPoint(_thisTransform.position);

            if (!bonusViewPortPosition.IsInVerticalViewportSpace()) 
                StopObject();
        }
    }
}