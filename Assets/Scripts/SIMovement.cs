using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIMovement : MonoBehaviour
    {
        protected const float SLOW_SPEED = 5f;
        protected const float BASIC_SPEED = 10f;
        protected const float FAST_SPEED = 15f;

        [SerializeField] protected MovementType _movementType;
        [SerializeField] protected float _currentMovementSpeed;

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
            EventsHandler.OnPlayerMove?.Invoke();
        }

    }
}
