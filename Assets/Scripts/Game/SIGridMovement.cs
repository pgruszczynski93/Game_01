using UnityEngine;

namespace SpaceInvaders
{
    public class SIGridMovement : SIMovement
    {
        [SerializeField] GridMovementSetup _gridMovementSetup;
        [SerializeField] GridMovementSettings _gridMovementSettings;

        [SerializeField] float _currentSpeedMultiplier;
        [Range(0f, 3f), SerializeField] protected float _screenEdgeOffset;
        [SerializeField] SIGridLimiter _gridLimiter;

        float _rightScreenEdgeOffset;
        float _leftScreenEdgeOffset;

        LocalGridMinMax _gridMinMax;
        ScreenEdges _screenEdges;

        protected override void Initialise()
        {
            base.Initialise();

            _canMove = true;
            _gridMovementSettings = _gridMovementSetup.gridMovementSettings;
            _screenEdges = SIGameMasterBehaviour.Instance.ScreenAreaCalculator.CalculateWorldLimits();
            _rightScreenEdgeOffset = _screenEdges.rightScreenEdge - _screenEdgeOffset;
            _leftScreenEdgeOffset = _screenEdges.leftScreenEdge + _screenEdgeOffset;
            _initialMovementSpeed = _gridMovementSettings.initialMovementSpeed;
            _currentMovementSpeed = _initialMovementSpeed;
            UpdateMovementOffsets();
        }

        protected override void AssignEvents()
        {
            SIEventsHandler.OnUpdate += TryToMoveObject;
            SIEventsHandler.OnEnemyDeath += UpdateMovementOffsets;
            SIEventsHandler.OnEnemySpeedMultiplierChanged += TryToUpdateCurrentGridMovementSpeed;
            SIEventsHandler.OnWaveEnd += ResetGridMovement;
        }

        void TryToUpdateCurrentGridMovementSpeed(float multiplier)
        {
            _currentSpeedMultiplier += multiplier;
        }

        protected override void RemoveEvents()
        {
            SIEventsHandler.OnUpdate -= TryToMoveObject;
            SIEventsHandler.OnEnemyDeath -= UpdateMovementOffsets;
            SIEventsHandler.OnEnemySpeedMultiplierChanged -= TryToUpdateCurrentGridMovementSpeed;
            SIEventsHandler.OnWaveEnd -= ResetGridMovement;
        }

        void UpdateMovementOffsets()
        {
            _gridMinMax = _gridLimiter.CalculateGridMinMax();
            _rightScreenEdgeOffset = _screenEdges.rightScreenEdge - _gridMinMax.localGridHorizontalMax -
                                     _gridMovementSettings.enemyWidthOffset;
            _leftScreenEdgeOffset = _screenEdges.leftScreenEdge - _gridMinMax.localGridHorizontalMin +
                                    _gridMovementSettings.enemyWidthOffset;
        }

        protected override void TryToMoveObject()
        {
            if (SIEnemiesGridsMaster.Instance.IsEnemyInGridMovementAllowed == false || _canMove == false)
                return;

            UpdatePosition();
            UpdateRotation();
        }

        protected override void TryToStopObject()
        {
            _canMove = false;
        }

        protected override void UpdatePosition()
        {
            _dt = Time.deltaTime;

            Vector3 currentPosition = _thisTransform.position;
            float horizontalMovementDelta = _dt * _currentSpeedMultiplier * _currentMovementSpeed;
            bool isInScreenBounds = IsInHorizontalScreenBounds(currentPosition);

            if (isInScreenBounds)
            {
                MoveObjectInScreenBounds(currentPosition, horizontalMovementDelta);
            }
            else
            {
                ClampObjectMovementPosition(currentPosition);
                UpdateMovementProperties();
            }
        }

        void ClampObjectMovementPosition(Vector3 currentPosition)
        {
            float clampedHorizontalPos =
                Mathf.Clamp(currentPosition.x, _leftScreenEdgeOffset, _rightScreenEdgeOffset);

            _thisTransform.position = new Vector3(
                clampedHorizontalPos,
                currentPosition.y - _gridMovementSettings.gridDownStep,
                currentPosition.z);
        }

        void MoveObjectInScreenBounds(Vector3 currentPosition, float horizontalMovementDelta)
        {
            Vector3 newPosition = new Vector3(currentPosition.x + horizontalMovementDelta,
                currentPosition.y);
            Vector3 smoothedPosition =
                Vector3.Lerp(currentPosition, newPosition, _gridMovementSettings.movementSmoothStep);
            _thisTransform.position = smoothedPosition;
        }

        protected override void UpdateRotation()
        {
            //Intentionally not implemented.
        }

        void UpdateMovementProperties()
        {
            _currentMovementSpeed = -_currentMovementSpeed;
            _currentSpeedMultiplier += _gridMovementSettings.speedMultiplierStep;
            _currentSpeedMultiplier = Mathf.Clamp(_currentSpeedMultiplier,
                _initialMovementSpeed,
                _gridMovementSettings.maxMovementSpeedMultiplier);
        }

        bool IsInHorizontalScreenBounds(Vector3 currentPosition)
        {
            float horizontalPosition = currentPosition.x;
            return horizontalPosition >= _leftScreenEdgeOffset && horizontalPosition <= _rightScreenEdgeOffset;
        }

        void ResetGridMovement()
        {
            _canMove = true;
            _currentMovementSpeed = Mathf.Abs(_currentMovementSpeed);
            _currentMovementSpeed += _gridMovementSettings.newWaveInitialSpeedChange;
            _currentMovementSpeed = Mathf.Clamp(_currentMovementSpeed, _initialMovementSpeed,
                _gridMovementSettings.maxMovementSpeed);
            _currentSpeedMultiplier = _gridMovementSettings.initialSpeedMultiplier;
            UpdateMovementOffsets();
        }
    }
}