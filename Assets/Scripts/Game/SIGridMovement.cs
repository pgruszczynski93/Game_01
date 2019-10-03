using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpaceInvaders
{
    public class SIGridMovement : MonoBehaviour, IUpdateTransform, ICanMove, IConfigurableObject
    {
        [SerializeField] private float _initialMovementSpeed;
        [SerializeField] private float _currentMovementSpeed;
        [SerializeField] private float _currentSpeedMultiplier;
        [SerializeField] private SIGridLimiter _gridLimiter;
        [SerializeField] private GridMovementSettings _gridMovementSettings;

        private bool _initialised;
        private bool _canMove;
        private float _dt;
        private float _rightScreenEdgeOffset;
        private float _leftScreenEdgeOffset;

        private LocalGridMinMax _gridMinMax;
        private ScreenEdges _screenEdges;
        private Transform _thisTransform;

        void Initialise()
        {
            if (_initialised)
                return;
            _initialised = true;

            _canMove = true;
            _thisTransform = transform;
            _initialMovementSpeed = _gridMovementSettings.initialMovementSpeed;
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
            _gridMinMax = _gridLimiter.CalculateGridMinMax();
            _rightScreenEdgeOffset = _screenEdges.rightScreenEdge - _gridMinMax.localGridHorizontalMax -
                                     _gridMovementSettings.enemyWidthOffset;
            _leftScreenEdgeOffset = _screenEdges.leftScreenEdge - _gridMinMax.localGridHorizontalMin +
                                    _gridMovementSettings.enemyWidthOffset;
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
                Vector3 smoothedPosition =
                    Vector3.Lerp(currentPosition, newPosition, _gridMovementSettings.smoothMovementStep);
                _thisTransform.position = smoothedPosition;
            }
            else
            {
                float clampedHorizontalPos =
                    Mathf.Clamp(currentPosition.x, _leftScreenEdgeOffset, _rightScreenEdgeOffset);

                _thisTransform.position = new Vector3(
                    clampedHorizontalPos,
                    currentPosition.y - _gridMovementSettings.gridDownStep,
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
            _currentSpeedMultiplier += _gridMovementSettings.speedMultiplierStep;
            _currentSpeedMultiplier = Mathf.Clamp(_currentSpeedMultiplier, _gridMovementSettings.movementSpeedMultiplierMin,
                _gridMovementSettings.movementSpeedMultiplierMax);
        }

        private bool IsInHorizontalScreenBounds(Vector3 currentPosition)
        {
            float horizontalPosition = currentPosition.x;
            return horizontalPosition >= _leftScreenEdgeOffset && horizontalPosition <= _rightScreenEdgeOffset;
        }

        private void ResetGridMovement()
        {
            _canMove = true;
            _initialMovementSpeed += _gridMovementSettings.newWaveInitialSpeedChange;
            _initialMovementSpeed = Mathf.Clamp(_initialMovementSpeed, _gridMovementSettings.initialMovementSpeedMin,
                _gridMovementSettings.initialMovementSpeedMax);
            _currentMovementSpeed = _initialMovementSpeed;
            _currentSpeedMultiplier = _gridMovementSettings.initialSpeedMultiplier;
            UpdateMovementOffsets();
        }
    }
}