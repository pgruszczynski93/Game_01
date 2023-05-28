using DG.Tweening;
using PG.Game.EventSystem;
using PG.Game.Features;
using UnityEngine;

namespace PG.Game.Entities {
    public abstract class MovementBehaviour : MonoBehaviour, IModifyTimeSpeedMultiplier {
        protected bool _canMove; //State of movement possibility
        protected bool _isMoving; //Is moving now?
        protected bool _initialised;

        [SerializeField] protected float _initialMovementSpeed;
        [SerializeField] protected float _currentMovementSpeed;

        protected float _dt;
        protected float _speedModificator;
        protected Transform _thisTransform;
        protected Tweener _tweener;

        public bool IsMoving => _isMoving;
        public Transform MovementTransform => _thisTransform;
        public Vector3 MovementWorldPosition => _thisTransform.position;

        protected abstract bool IsMovementPossible();
        protected abstract void UpdatePosition();
        protected abstract void UpdateRotation();
        public abstract void SetTimeSpeedModifier(float timeSpeedModifier, float progress = 1f);

        protected virtual void Initialise() {
            if (_initialised)
                return;

            _initialised = true;
            _canMove = false;
            _thisTransform = transform;
            RequestTimeSpeedModification();
        }

        protected void Start() {
            Initialise();
        }

        protected void OnEnable() {
            SubscribeEvents();
        }

        protected void OnDisable() {
            UnsubscribeEvents();
        }

        protected virtual void SubscribeEvents() {
            GeneralEvents.OnUpdate += HandleOnUpdate;
        }

        protected virtual void UnsubscribeEvents() {
            GeneralEvents.OnUpdate -= HandleOnUpdate;
        }

        protected virtual void HandleOnUpdate() {
            if (!IsMovementPossible()) {
                DisableMovingState();
            }
            else {
                EnableMovingState();
                UpdatePosition();
                UpdateRotation();
            }
        }

        void EnableMovingState() {
            if (!_isMoving)
                _isMoving = true;
        }

        void DisableMovingState() {
            if (_isMoving)
                _isMoving = false;
        }

        public void EnableMovement() {
            if (!_canMove)
                _canMove = true;
        }

        public void DisableMovement() {
            if (_canMove)
                _canMove = false;
        }

        public void RequestTimeSpeedModification() {
            GameplayEvents.BroadcastOnSpeedModificationRequested(this);
        }
    }
}