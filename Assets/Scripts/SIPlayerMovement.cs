using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class SIPlayerMovement : SIMovement, IMoveable
    {
        private const float SLOW_SPEED = 15f;
        private const float BASIC_SPEED = 30f;
        private const float FAST_SPEED = 45f;
        private const float MAX_ROTATION_ANGLE = 40;

        protected Dictionary<MovementType, float> _movementSpeeds;

        public float InputMovementValue { get; set; }

        protected override void OnEnable()
        {
            SIEventsHandler.OnObjectMovement += MoveObj;
        }

        protected override void OnDisable()
        {
            SIEventsHandler.OnObjectMovement -= MoveObj;
        }

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();

            _movementSpeeds = new Dictionary<MovementType, float>
            {
                {MovementType.Basic, BASIC_SPEED},
                {MovementType.Fast, FAST_SPEED},
                {MovementType.Slow, SLOW_SPEED}
            };

            _currentMovementSpeed = _movementSpeeds[0];
        }

        private void SetMovementSpeed(MovementType movementType)
        {
            if (_movementSpeeds.TryGetValue(movementType, out float currentSpeed) == false)
            {
                Debug.Log("No key in _movementSpeeds dictionary - current speed setup with default.");
                _currentMovementSpeed = BASIC_SPEED;
                return;
            }

            _currentMovementSpeed = _movementSpeeds[movementType];
        }

        public void MoveObj()
        {
            InputMovementValue = Input.GetAxis("Horizontal");

            MoveObject(InputMovementValue);
            RotateObject(InputMovementValue);
        }

        private void RotateObject(float rotateValue)
        {
            Quaternion fromRotation = _cachedTransform.rotation;
            Quaternion toRotation = Quaternion.Euler(0, -rotateValue * MAX_ROTATION_ANGLE, 0);
            _cachedTransform.rotation = Quaternion.Slerp(fromRotation, toRotation, _lerpStep);
        }
    }
}