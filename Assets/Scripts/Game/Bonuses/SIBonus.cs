using System;
using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(SIBonusColliderBehaviour))]
    public abstract class SIBonus : MonoBehaviour, ICanMove
    {
        [SerializeField] protected BonusSetup _bonusSetup;
        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] protected MeshRenderer _meshRenderer;
        [SerializeField] protected Collider _mainCollider;

        [SerializeField] Transform _parent;
        
        protected BonusSettings _bonusSettings;

        Transform _thisTransform;

        public abstract BonusSettings GetBonusSettings();
        
        void Initialise()
        {
            _thisTransform = transform;
            _bonusSettings = _bonusSetup.bonusSettings;
            ManageBonusInteraction(false);
        }

        void Start()
        {
            Initialise();
        }

        void OnEnable()
        {
            SubscribeEvents();
        }

        void OnDisable()
        {
            UnsubscribeEvents();
        }

        protected void SubscribeEvents()
        {
            SIEventsHandler.OnUpdate += TryToResetObject;
        }

        protected void UnsubscribeEvents()
        {
            SIEventsHandler.OnUpdate -= TryToResetObject;
        }

        void ManageBonusInteraction(bool isEnabled)
        {
            _meshRenderer.enabled = isEnabled;
            _mainCollider.enabled = isEnabled;
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