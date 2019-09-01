using System;
using UnityEngine;

namespace SpaceInvaders
{
    public abstract class SIMovement : MonoBehaviour
    {
        protected const float VERTICAL_MOVEMENT_VIEWPORT_STEP = 0.1f;

        [SerializeField] protected bool _canMove;
        [SerializeField] protected float _initialMovementSpeed;
        [SerializeField] protected float _currentMovementSpeed;
        [SerializeField] protected MovementDirection _movementDirection;
        [Range(0, 3), SerializeField] protected float _screenEdgeOffset;
        [Range(0, 1), SerializeField] protected float _smoothMovementStep;
        [Range(0, 50), SerializeField] protected float _maxRotationAngle;
        
        private bool _initialised;
        
        protected float _dt;
        protected Vector2 _startPosition;
        protected Quaternion _fromRotation;
        protected Quaternion _toRotation;
        protected Transform _cachedTransform;
        protected Camera _mainCamera;

        protected void Start()
        {
            Initialise();
        }

        protected virtual void Initialise()
        {
            if (_initialised)
                return;

            _initialised = true;
            _canMove = true;
            _mainCamera = SIGameMasterBehaviour.Instance.MainCamera;
            
            _cachedTransform = transform;
            _startPosition = _cachedTransform.localPosition;
            _initialMovementSpeed = _currentMovementSpeed;
        }

        protected virtual void OnEnable(){}
        protected virtual void OnDisable() {}
        protected virtual void ResetMovement(){}
        protected abstract void UpdatePosition();
        protected abstract void UpdateRotation();
    }
}
