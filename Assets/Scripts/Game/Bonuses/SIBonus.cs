using System;
using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(SIBonusColliderBehaviour))]
    public abstract class SIBonus : MonoBehaviour, ICanMove
    {
        public static event Action OnBonusEnabled;
        
        [SerializeField] protected BonusSetup _bonusSetup;
        [SerializeField] protected Rigidbody _rigidbody;
        protected BonusSettings _bonusSettings;

        bool _initialised;
        Transform _thisTransform;
        Vector3 _startPosition;

        public abstract BonusSettings GetBonusSettings();
        protected virtual void Initialise()
        {
            if (_initialised)
                return;
            _initialised = true;
            _thisTransform = transform;
            _startPosition = _thisTransform.position;
            _bonusSettings = _bonusSetup.bonusSettings;
        }

        void Start()
        {
            Initialise();
        }

        void OnEnable()
        {
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        protected void AssignEvents()
        {
            SIEventsHandler.OnUpdate += TryToResetObject;
        }

        protected void RemoveEvents()
        {
            SIEventsHandler.OnUpdate -= TryToResetObject;
        }

        protected void TryToResetObject()
        {
            Vector3 bonusViewPortPosition =
                SIGameMasterBehaviour.Instance.MainCamera.WorldToViewportPoint(_thisTransform.position);

            if (bonusViewPortPosition.IsInVerticalViewportSpace()) 
                StopObject();
        }

        public void MoveObject()
        {
            _rigidbody.AddForce(SIHelpers.VectorDown * _bonusSettings.bonusProperties.releaseForceMultiplier, ForceMode.Impulse);
        }

        public void StopObject()
        {
            _rigidbody.velocity = SIHelpers.VectorZero;
            _thisTransform.position = _startPosition;
            gameObject.SetActive(false);
        }

        public void BroadcastOnBonusEnabled()
        {
            OnBonusEnabled?.Invoke();
        }
    }
}