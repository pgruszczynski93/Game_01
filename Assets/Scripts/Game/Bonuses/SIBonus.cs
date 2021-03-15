using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    [RequireComponent(typeof(SIBonusColliderBehaviour))]
    public class SIBonus : MonoBehaviour, ICanMove {
        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] protected GameObject _bonusRoot;

        [SerializeField] Transform _parent;
        [SerializeField] SIBonusDictionary _bonusesVariants;

        BonusType _bonusType;
        BonusSettings _bonusSettings;
        Transform _thisTransform;

        void OnEnable() {
            SubscribeEvents();
        }

        void OnDisable() {
            UnsubscribeEvents();
        }

        void SubscribeEvents() {
            SIEventsHandler.OnUpdate += TryToResetObject;
        }

        void UnsubscribeEvents() {
            SIEventsHandler.OnUpdate -= TryToResetObject;
        }

        void EnableBonus(bool isEnabled) {
            _bonusesVariants[_bonusType].bonusRenderer.enabled = true;
            _bonusRoot.SetActive(isEnabled);
        }

        public void SetBonus(BonusType bonusType) {
            if (_thisTransform == null) 
                _thisTransform = transform;

            _bonusType = bonusType;
            _bonusSettings = _bonusesVariants[_bonusType].scriptableBonus.bonusSettings;
            EnableBonus(false);
        }

        public BonusSettings GetBonusSettings() {
            return _bonusSettings;
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

        void TryToResetObject() {
            Vector3 bonusViewPortPosition =
                SIGameMasterBehaviour.Instance.MainCamera.WorldToViewportPoint(_thisTransform.position);

            if (!bonusViewPortPosition.IsInVerticalViewportSpace()) StopObject();
        }

        void ReleaseObject() {
            _thisTransform.SetParent(null);
            _rigidbody.AddForce(SIHelpers.VectorDown * _bonusSettings.bonusProperties.releaseForceMultiplier,
                ForceMode.Impulse);
        }
    }
}