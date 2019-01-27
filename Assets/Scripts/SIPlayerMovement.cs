using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class SIPlayerMovement : SIMovement, IMoveable
    {
        private const float SLOW_SPEED = 15f;
        private const float FAST_SPEED = 45f;

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
            BASIC_SPEED = 30f;
            _currentMovementSpeed = BASIC_SPEED;
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
        }
    }
}