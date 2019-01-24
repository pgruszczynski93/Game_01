using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIMovement : MonoBehaviour
    {
        protected const float SLOW_SPEED = 15f;
        protected const float BASIC_SPEED = 30f;
        protected const float FAST_SPEED = 45f;

        protected const float CAMERA_MIN_PERCENT_OFFSET = 0.05f;
        protected const float CAMERA_MAX_PERCENT_OFFSET = 0.95f;

        [SerializeField] protected MovementType _movementType;
        [SerializeField] protected float _currentMovementSpeed;
        [SerializeField] protected Camera _mainCamera;

        protected Dictionary<MovementType, float> _movementSpeeds;

        public MovementType CurrentMovementType
        {
            set { _movementType = value; }
            get { return _movementType; }
        }

        protected virtual void Awake()
        {
            SetInitialReferences();
        }

        protected virtual void SetInitialReferences()
        {
            _movementSpeeds = new Dictionary<MovementType, float>
            {
                {MovementType.Basic, BASIC_SPEED},
                {MovementType.Fast, FAST_SPEED},
                {MovementType.Slow, SLOW_SPEED}
            };

            _currentMovementSpeed = BASIC_SPEED;
        }

        protected virtual void OnEnable(){}

        protected virtual void OnDisable() {}

        protected virtual void MoveObject() {}

        protected void SetMovementSpeed(MovementType movementType)
        {
            if (_movementSpeeds.TryGetValue(movementType, out float currentSpeed) == false)
            {
                Debug.Log("No key in _movementSpeeds dictionary - current speed setup with default.");
                _currentMovementSpeed = BASIC_SPEED;
                return;
            }

            _currentMovementSpeed = _movementSpeeds[movementType];
        }

        protected virtual void Update()
        {
            SIEventsHandler.OnPlayerMove?.Invoke();
        }

    }
}
