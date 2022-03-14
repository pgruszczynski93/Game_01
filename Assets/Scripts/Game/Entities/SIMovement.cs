using DG.Tweening;
using UnityEngine;

namespace SpaceInvaders {
    public abstract class SIMovement : MonoBehaviour, IModifyTimeSpeedMultiplier {
        protected bool _canMove;
        protected bool _initialised;

        [SerializeField] protected float _initialMovementSpeed;
        [SerializeField] protected float _currentMovementSpeed;

        protected float _dt;
        protected float _speedModificator;
        protected Transform _thisTransform;
        protected Tweener _tweener;

        public Transform MovementTransform => _thisTransform;
        public Vector3 MovementWorldPosition => _thisTransform.position;

        protected abstract void UpdatePosition();
        protected abstract void UpdateRotation();
        protected virtual void TryToMoveObject() { }
        protected virtual void TryToStopObject() { }
        protected virtual void ResetMovement() { }

        protected virtual void Initialise() {
            if (_initialised)
                return;

            _initialised = true;
            _thisTransform = transform;
            RequestTimeSpeedModification();
        }

        protected void Start() => Initialise();
        protected void OnEnable() => SubscribeEvents();
        protected void OnDisable() => UnsubscribeEvents();

        protected virtual void SubscribeEvents() {
            SIEventsHandler.OnUpdate += HandleOnUpdate;
        }

        protected virtual void UnsubscribeEvents() {
            SIEventsHandler.OnUpdate -= HandleOnUpdate;
        }

        protected virtual void HandleOnUpdate() {
            TryToMoveObject();
        }

        public void SetTimeSpeedModifier(float timeSpeedModifier, float progress = 1f) {
            _speedModificator = timeSpeedModifier;
        }
        
        public void RequestTimeSpeedModification() {
            SIGameplayEvents.BroadcastOnSpeedModificationRequested(this);
        }
    }
}