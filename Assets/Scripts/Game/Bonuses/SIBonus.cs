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
        MeshRenderer _currentBonusVariantRenderer;
        MeshRenderer _lastRenderer;
        Transform _thisTransform;
        
        public BonusSettings BonusVariantSettings => _currentVariantSettings;
        public MeshRenderer BonusVariantRenderer => _currentBonusVariantRenderer;

        void OnEnable() => SubscribeEvents();

        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents() {
            SIEventsHandler.OnUpdate += TryToResetObject;
        }

        void UnsubscribeEvents() {
            SIEventsHandler.OnUpdate -= TryToResetObject;
        }

        public void SetAndReleaseBonusVariant(Vector3 releasePos, BonusType bonusType) {
            if (_thisTransform == null)
                _thisTransform = transform;
            
            _currentDropPos = releasePos;
            _bonusType = bonusType;
            _currentVariantSettings = _bonusesVariants[_bonusType].scriptableBonus.bonusSettings;
            _currentBonusVariantRenderer = _bonusesVariants[_bonusType].bonusRenderer;
            
            _animatorController.SetRendererToAnimate(_currentBonusVariantRenderer);
            MoveObject();
        }

        public void MoveObject() {
            EnableBonus(true);
            ReleaseObject();
        }

        public void StopObject() {
            _rigidbody.velocity = SIHelpers.VectorZero;
            _thisTransform.SetParent(_parent);
            _thisTransform.localPosition = SIScreenUtils.HiddenObjectPosition;
            EnableBonus(false);
        }

        void EnableBonus(bool isEnabled) {

            if (_lastRenderer != null && _currentBonusVariantRenderer != _lastRenderer) {
                _lastRenderer.enabled = !isEnabled;
            }

            _currentBonusVariantRenderer.enabled = isEnabled;
            _bonusRoot.SetActive(isEnabled);
            _lastRenderer = _currentBonusVariantRenderer;
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
            _rigidbody.AddForce(SIHelpers.VectorDown * _currentVariantSettings.releaseForceMultiplier,
                ForceMode.Impulse);
        }
    }
}