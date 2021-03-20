using System;
using UnityEngine;

namespace SpaceInvaders {
    [RequireComponent(typeof(SIBonusColliderBehaviour))]
    public class SIBonus : MonoBehaviour, ICanMove {
        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] protected GameObject _bonusRoot;

        [SerializeField] Transform _parent;
        [SerializeField] SIBonusDictionary _bonusesVariants;

        BonusType _bonusType;
        Vector3 _currentDropPos;
        
        BonusSettings _currentSettings;
        MeshRenderer _currentRenderer;
        MeshRenderer _lastRenderer;
        Transform _thisTransform;
        
        public BonusSettings BonusSettings => _currentSettings;
        public MeshRenderer BonusRenderer => _currentRenderer;

        void Start() => Initialise();
        void OnEnable() => SubscribeEvents();

        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents() {
            SIEventsHandler.OnUpdate += TryToResetObject;
        }

        void UnsubscribeEvents() {
            SIEventsHandler.OnUpdate -= TryToResetObject;
        }
        
        void Initialise() {
            _thisTransform = transform;
        }

        public void SetAndReleaseBonusVariant(Vector3 releasePos, BonusType bonusType) {
            _currentDropPos = releasePos;
            _bonusType = bonusType;
            _currentSettings = _bonusesVariants[_bonusType].scriptableBonus.bonusSettings;
            _currentRenderer = _bonusesVariants[_bonusType].bonusRenderer;
            MoveObject();
        }

        public void MoveObject() {
            EnableBonus(true);
            ReleaseObject();
        }

        public void StopObject() {
            _rigidbody.velocity = SIHelpers.VectorZero;
            _thisTransform.SetParent(_parent);
            _thisTransform.localPosition = SIScreenUtils.HiddenObjectPosition;;
            EnableBonus(false);
        }

        void EnableBonus(bool isEnabled) {

            if (_lastRenderer != null && _currentRenderer != _lastRenderer) {
                _lastRenderer.enabled = !isEnabled;
            }

            _currentRenderer.enabled = isEnabled;
            _bonusRoot.SetActive(isEnabled);
            _lastRenderer = _currentRenderer;
        }

        void TryToResetObject() {
            Vector3 bonusViewPortPosition =
                SIGameMasterBehaviour.Instance.MainCamera.WorldToViewportPoint(_thisTransform.position);

            if (!bonusViewPortPosition.IsInVerticalViewportSpace()) 
                StopObject();
        }

        void ReleaseObject() {
            _thisTransform.SetParent(null);
            _thisTransform.position = _currentDropPos;
            _rigidbody.AddForce(SIHelpers.VectorDown * _currentSettings.releaseForceMultiplier,
                ForceMode.Impulse);
        }
    }
}