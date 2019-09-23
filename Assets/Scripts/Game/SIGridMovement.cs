using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceInvaders
{
    public class SIGridMovement : MonoBehaviour, IUpdateTransform, ICanMove, IConfigurableObject
    {
        [Range(1, 10), SerializeField] private float _initialMovementSpeed;
        [Range(1, 10), SerializeField] private float _initialSpeedMultiplier;
        [Range(0, 5), SerializeField] private float _newWaveInitialSpeedChange;
        [Range(0, 1), SerializeField] private float _gridDownStep;
        [Range(0, 2), SerializeField] private float _speedMultiplierStep;
        [Range(0, 1), SerializeField] private float _smoothMovementStep;
        [SerializeField] private float _currentMovementSpeed;
        [SerializeField] private float _currentSpeedMultiplier;
        [SerializeField] private SIGridLimiter _gridLimiter;
        [SerializeField] private GridMovementSettings _gridMovementSettings;

        private bool _initialised;
        private bool _canMove;
        private float _dt;
        private float _rightScreenEdgeOffset;
        private float _leftScreenEdgeOffset;
        
        private ScreenEdges _screenEdges;
        private Transform _thisTransform;

        void Initialise()
        {
            if (_initialised)
                return;
            _initialised = true;

            _canMove = true;
            _thisTransform = transform;
            _currentMovementSpeed = _initialMovementSpeed;
            _screenEdges = SIGameMasterBehaviour.Instance.ScreenAreaCalculator.CalculateWorldLimits();
            UpdateMovementOffsets();
        }

        public void Configure(ScriptableSettingsMaster settings)
        {
            _gridMovementSettings = settings.gridMovementConfigurator.gridMovementSettings;
        }

        void Start()
        {
            Initialise();
        }

        private void OnEnable()
        {
            AssignEvents();
        }

        private void OnDisable()
        {
            RemoveEvents();
        }

        private void AssignEvents()
        {
            SIEventsHandler.OnUpdate += MoveObject;
            SIEventsHandler.OnEnemyDeath += UpdateMovementOffsets;
//            SIEventsHandler.OnEnemySpeedMultiplierChanged += UpdateMovementStep;
            SIEventsHandler.OnWaveEnd += ResetGridMovement;
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnUpdate -= MoveObject;
            SIEventsHandler.OnEnemyDeath -= UpdateMovementOffsets;
//            SIEventsHandler.OnEnemySpeedMultiplierChanged -= UpdateMovementStep;
            SIEventsHandler.OnWaveEnd -= ResetGridMovement;
        }
        
        private void UpdateMovementOffsets()
        {
            float halfGridLength = _gridLimiter.HalfGridLength();
            _rightScreenEdgeOffset = _screenEdges.rightScreenEdge - halfGridLength;
            _leftScreenEdgeOffset = _screenEdges.leftScreenEdge + halfGridLength;
        }

        public void MoveObject()
        {
            if (SIEnemiesGridsMaster.Instance.IsEnemyInGridMovementAllowed == false || _canMove == false)
                return;

            UpdatePosition();
            UpdateRotation();
        }

        public void StopObject()
        {
            _canMove = false;
        }

        public void UpdatePosition()
        {
            _dt = Time.deltaTime;
            
            Vector3 currentPosition = _thisTransform.position;

            float horizontalMovementDelta = _dt * _currentSpeedMultiplier * _currentMovementSpeed;
            bool isInScreenBounds = IsInHorizontalScreenBounds(currentPosition);

            if (isInScreenBounds)
            {
                Vector3 newPosition = new Vector3(currentPosition.x + horizontalMovementDelta,
                    currentPosition.y);
                Vector3 smoothedPosition = Vector3.Lerp(currentPosition, newPosition, _smoothMovementStep);
                _thisTransform.position = smoothedPosition;
            }
            else
            {
                float clampedHorizontalPos =
                    Mathf.Clamp(currentPosition.x, _leftScreenEdgeOffset, _rightScreenEdgeOffset);

                _thisTransform.position = new Vector3(
                    clampedHorizontalPos,
                    currentPosition.y - _gridDownStep,
                    currentPosition.z);


                UpdateMovementProperties();
            }
        }

        public void UpdateRotation()
        {
            //Intentionally not implemented.
        }

        void UpdateMovementProperties()
        {
            _currentMovementSpeed = -_currentMovementSpeed;
            _currentSpeedMultiplier += _speedMultiplierStep;
        }
        
        private bool IsInHorizontalScreenBounds(Vector3 currentPosition)
        {
            float horizontalPosition = currentPosition.x;
            return horizontalPosition >= _leftScreenEdgeOffset && horizontalPosition <= _rightScreenEdgeOffset;
        }
        
        private void ResetGridMovement()
        {
            _canMove = true;
            _initialMovementSpeed += _initialSpeedMultiplier;
            _currentMovementSpeed = _initialMovementSpeed;
            _currentSpeedMultiplier = _initialSpeedMultiplier;
        }
        
    }
}