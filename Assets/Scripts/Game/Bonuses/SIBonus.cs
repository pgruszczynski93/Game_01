using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceInvaders {
    [RequireComponent(typeof(SIBonusColliderBehaviour))]
    public class SIBonus : MonoBehaviour, ICanMove {
        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] protected GameObject _bonusRoot;

        [SerializeField] Transform _parent;
        [SerializeField] SIBonusDictionary _bonusesVariants;

        BonusType _bonusType;
        BonusSettings _currentSettings;
        MeshRenderer _currentRenderer;
        MeshRenderer _lastRenderer;
        Transform _thisTransform;
        
        public BonusSettings BonusSettings => _currentSettings;
        public MeshRenderer BonusRenderer => _currentRenderer;

        void OnEnable() => SubscribeEvents();

        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents() {
            SIEventsHandler.OnUpdate += TryToResetObject;
        }

        void UnsubscribeEvents() {
            SIEventsHandler.OnUpdate -= TryToResetObject;
        }
        
        public void SetBonusVariant(BonusType bonusType = BonusType.Life) {
            if (_thisTransform == null) 
                _thisTransform = transform;

            _bonusType = bonusType;
            
            if(!_bonusesVariants.TryGetValue(_bonusType, out SIBonusData data))
                _bonusesVariants.Add(_bonusType, data);
            
            _currentSettings = _bonusesVariants[_bonusType].scriptableBonus.bonusSettings;
            _currentRenderer = _bonusesVariants[_bonusType].bonusRenderer;
        }

        public void MoveObject() {
            EnableBonus(true);
            ReleaseObject();
        }

        public void StopObject() {
            _rigidbody.velocity = SIHelpers.VectorZero;
            _thisTransform.SetParent(_parent);
            _thisTransform.localPosition = SIHelpers.VectorZero;
            EnableBonus(false);
        }

        void EnableBonus(bool isEnabled) {
            
            if (_lastRenderer != null && _currentRenderer != _lastRenderer)
                _lastRenderer.enabled = !isEnabled;

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
            _rigidbody.AddForce(SIHelpers.VectorDown * _currentSettings.releaseForceMultiplier,
                ForceMode.Impulse);
        }
        
        #if UNITY_EDITOR
        [Button]
        void BonusSelectionTest(){
            var totalBonuses = Enum.GetValues(typeof(BonusType)).Length;
            var selected = Random.Range(0, totalBonuses);
            SetBonusVariant((BonusType)selected);
            EnableBonus(true);
        }
        #endif
    }
}