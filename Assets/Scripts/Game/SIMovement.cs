using System;
using UnityEngine;

namespace SpaceInvaders
{
    public abstract class SIMovement : MonoBehaviour
    {
        [SerializeField] protected bool _canMove;
        [SerializeField] protected float _initialMovementSpeed;
        [SerializeField] protected float _currentMovementSpeed;
        [Range(0, 3), SerializeField] protected float _screenEdgeOffset;
        [Range(0, 1), SerializeField] protected float _smoothMovementStep;
        [Range(0, 50), SerializeField] protected float _maxRotationAngle;
        
        private bool _initialised;
        
        protected float _dt;
        protected float _rightScreenOffset;
        protected float _leftScreenOffset;
        protected ScreenEdges _screenEdges;
        protected Vector2 _startPosition;
        protected Quaternion _fromRotation;
        protected Quaternion _toRotation;
        protected Transform _cachedTransform;

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
            
            _cachedTransform = transform;
            _startPosition = _cachedTransform.localPosition;
            _initialMovementSpeed = _currentMovementSpeed;
            _screenEdges = SIGameMasterBehaviour.Instance.ScreenAreaCalculator.CalculateWorldLimits();
            _rightScreenOffset = _screenEdges.rightScreenEdge - _screenEdgeOffset;
            _leftScreenOffset = _screenEdges.leftScreenEdge + _screenEdgeOffset;
        }

        protected virtual void OnEnable(){}
        protected virtual void OnDisable() {}
        protected virtual void ResetMovement(){}
        protected abstract void UpdatePosition();
        protected abstract void UpdateRotation();
    }
}
