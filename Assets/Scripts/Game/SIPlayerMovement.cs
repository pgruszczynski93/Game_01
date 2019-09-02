using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MinesEvader;

namespace SpaceInvaders
{
    public class SIPlayerMovement : SIMovement, ICanMove
    {
        private Vector3 _inputVector;
        private Dictionary<MovementType, float> _movementSpeeds;

        protected override void Initialise()
        {
            base.Initialise();

            _movementSpeeds = new Dictionary<MovementType, float>
            {
                {MovementType.Slow, 15f},
                {MovementType.Basic, 20f},
                {MovementType.Fast, 25f},
            };


            _currentMovementSpeed = _movementSpeeds[0];
        }

        protected override void OnEnable()
        {
            AssignEvents();
        }

        protected override void OnDisable()
        {
            RemoveEvents();
        }

        private void AssignEvents()
        {
            SIEventsHandler.OnUpdate += MoveObject;
            SIEventsHandler.OnInputCollected += HandleInputCollected;
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnUpdate -= MoveObject;
            SIEventsHandler.OnInputCollected += HandleInputCollected;
        }

        void HandleInputCollected(Vector3 inputVector)
        {
            _inputVector = inputVector;
        }

        #region UNUSED_METHODS

//        private void SetMovementSpeed(MovementType movementType)
//        {
//            if (_movementSpeeds.TryGetValue(movementType, out float currentSpeed) == false)
//            {
//                SIHelpers.SISimpleLogger(this, "No key in _movementSpeeds dictionary - current speed setup with default.", SimpleLoggerTypes.Error);
//                _currentMovementSpeed = BASIC_SPEED;
//                return;
//            }
//
//            _currentMovementSpeed = _movementSpeeds[movementType];
//        }

        #endregion

        public void MoveObject()
        {
            if (_canMove == false)
                return;

            UpdatePosition();
            UpdateRotation();
        }

        public void StopObject()
        {
            //todo: stop conditiions

            if (_canMove)
                return;

            _canMove = false;
        }

        protected override void UpdatePosition()
        {
            _dt = Time.deltaTime;
            float horizontalMovementDelta = _dt * _inputVector.x * _currentMovementSpeed;
            float verticalMovementDelta = _dt * _inputVector.y * _currentMovementSpeed;

            Vector3 currentPosition = _cachedTransform.position;
            Vector3 newPosition = new Vector3(currentPosition.x + horizontalMovementDelta,
                currentPosition.y /* + verticalMovementDelta*/);
            Vector3 smoothedPosition = Vector3.Lerp(currentPosition, newPosition, _smoothMovementStep);

            float clampedHorizontalPos =
                Mathf.Clamp(smoothedPosition.x, _leftScreenOffset, _rightScreenOffset);

            //NOTE: for now vertical movement is locked - remove it when necessary and tweak.
            
//            float clampedVerticalPos = Mathf.Clamp(smoothedPosition.y, _screenEdges.bottomScreenEdge,
//                _screenEdges.topScreenEdge);
            float clampedVerticalPos = smoothedPosition.y;

            smoothedPosition = new Vector3(clampedHorizontalPos, clampedVerticalPos, smoothedPosition.z);
            _cachedTransform.position = smoothedPosition;
        }

        protected override void UpdateRotation()
        {
            _fromRotation = _cachedTransform.rotation;
            _toRotation = Quaternion.Euler(0, -_inputVector.x * _maxRotationAngle, 0);
            _cachedTransform.rotation = Quaternion.Slerp(_fromRotation, _toRotation, _smoothMovementStep);
        }
    }
}