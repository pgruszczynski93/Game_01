using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class SIPlayerMovement : SIMovement, IMoveable
    {
        private float SLOW_SPEED = 15f;
        private float BASIC_SPEED = 30f;
        private float FAST_SPEED = 45f;
        private const float MAX_ROTATION_ANGLE = 40;

        [SerializeField] private float _touchTreshold;
        private Touch _mainTouch;
        private Vector2 _normalizedTouchDelta;
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

#if UNITY_ANDROID && !UNITY_EDITOR
            SLOW_SPEED = 7.5f;
            BASIC_SPEED = 12.5f;
            FAST_SPEED = 20f;
#endif
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
#if UNITY_EDITOR
            InputMovementValue = Input.GetAxis("Horizontal");

#elif UNITY_ANDROID
            CalculateMobileInputValue();
#endif
            MoveObject(InputMovementValue);
            RotateObject(InputMovementValue);
        }

        private void CalculateMobileInputValue()
        {
            if (Input.touchCount > 0)
            {
                _mainTouch = Input.GetTouch(0);
                _normalizedTouchDelta = _mainTouch.deltaPosition.normalized;

                if ((_mainTouch.phase == TouchPhase.Began || _mainTouch.phase == TouchPhase.Moved)
                    && _normalizedTouchDelta.magnitude > _touchTreshold)
                {
                    _normalizedTouchDelta = _mainTouch.deltaPosition.normalized;
                    InputMovementValue = _normalizedTouchDelta.x;
                }
                else
                {
                    InputMovementValue = 0f;
                }
            }
        }

        private void RotateObject(float rotateValue)
        {
            Quaternion fromRotation = _cachedTransform.rotation;
            Quaternion toRotation = Quaternion.Euler(0, -rotateValue * MAX_ROTATION_ANGLE, 0);
            _cachedTransform.rotation = Quaternion.Slerp(fromRotation, toRotation, _lerpStep);
        }
    }
}