using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(SIBonusColliderBehaviour))]
    public class SIBonus : MonoBehaviour, ICanMove
    {
        [SerializeField] protected BonusSetup _bonusSetup;
        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] protected GameObject _bonusRoot;

        [SerializeField] Transform _parent;
        // przerobić to na slownik <typ, bonuscoś{assety . mesh}>
        [SerializeField] MeshRenderer[] _availableMeshes;
        
        protected BonusSettings _bonusSettings;

        Transform _thisTransform;

        public BonusSettings GetBonusSettings() {
            return _bonusSettings;
        }
        
        void Initialise()
        {
            _thisTransform = transform;
            _bonusSettings = _bonusSetup.bonusSettings;
            ManageBonusInteraction(false);
        }

        void Start() => Initialise();
        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();

        void SubscribeEvents()
        {
            SIEventsHandler.OnUpdate += TryToResetObject;
        }

        void UnsubscribeEvents()
        {
            SIEventsHandler.OnUpdate -= TryToResetObject;
        }

        void ManageBonusInteraction(bool isEnabled)
        {
            _bonusRoot.SetActive(isEnabled);
        }

        void TryToResetObject()
        {
            Vector3 bonusViewPortPosition =
                SIGameMasterBehaviour.Instance.MainCamera.WorldToViewportPoint(_thisTransform.position);

            if (!bonusViewPortPosition.IsInVerticalViewportSpace()) 
                StopObject();
        }

        void ReleaseObject()
        {
            _thisTransform.SetParent(null);
            _rigidbody.AddForce(SIHelpers.VectorDown * _bonusSettings.bonusProperties.releaseForceMultiplier, ForceMode.Impulse);
        }
        
        public void MoveObject()
        {
            ManageBonusInteraction(true);
            ReleaseObject();
        }

        public void StopObject()
        {
            _rigidbody.velocity = SIHelpers.VectorZero;
            _thisTransform.SetParent(_parent);
            _thisTransform.localPosition = SIHelpers.VectorZero;
            ManageBonusInteraction(false);
        }
    }
}