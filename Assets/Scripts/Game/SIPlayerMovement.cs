using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class SIPlayerMovement : SIMovement, IMoveable
    {
        private float SLOW_SPEED = 20f;
        private float BASIC_SPEED = 25f;
        private float FAST_SPEED = 30f;

        private Touch _mainTouch;
        private Vector2 _normalizedTouchDelta;
        private Dictionary<MovementType, float> _movementSpeeds;
        [SerializeField] private Joystick _joystick;

        public float InputMovementValue { get; set; }

        protected override void OnEnable()
        {
            SIEventsHandler.OnObjectsMovement += MoveObj;
        }

        protected override void OnDisable()
        {
            SIEventsHandler.OnObjectsMovement -= MoveObj;
        }

        protected override void Initialize()
        {
            base.Initialize();

            MAX_ROTATION_ANGLE = 40f;
//#if UNITY_ANDROID && !UNITY_EDITOR
//            SLOW_SPEED = 7.5f;
//            BASIC_SPEED = 12.5f;
//            FAST_SPEED = 20f;
//#endif
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
                SIHelpers.SISimpleLogger(this, "No key in _movementSpeeds dictionary - current speed setup with default.", SimpleLoggerTypes.Error);
                _currentMovementSpeed = BASIC_SPEED;
                return;
            }

            _currentMovementSpeed = _movementSpeeds[movementType];
        }

        public void MoveObj()
        {
#if UNITY_EDITOR
            InputMovementValue = Input.GetAxis("Horizontal");

#elif UNITY_ANDROID && !UNITY_EDITOR
            CalculateMobileInputValue();
#endif
            MoveObject(InputMovementValue);
            RotateObject(InputMovementValue);
        }

        private void CalculateMobileInputValue()
        {
            if (_joystick == null)
            {
                return;
            }

            InputMovementValue = _joystick.Horizontal;
        }

        private void RotateObject(float rotateValue)
        {
            _fromRotation = _cachedTransform.rotation;
            _toRotation = Quaternion.Euler(0, -rotateValue * MAX_ROTATION_ANGLE, 0);
            _cachedTransform.rotation = Quaternion.Slerp(_fromRotation, _toRotation, _lerpStep);
        }

        public void StopObj()
        {
            throw new System.NotImplementedException();
        }
    }
}