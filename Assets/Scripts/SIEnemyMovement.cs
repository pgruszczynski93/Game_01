using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyMovement : SIMovement, IMoveable
    {
        private float _moveStep;

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _moveStep = 2;
        }

        protected override void OnEnable()
        {
            SIEventsHandler.OnObjectMovement += MoveObj;
            SIEventsHandler.OnEnemySpeedMultiplierChanged += UpdateMovementStep;
        }

        protected override void OnDisable()
        {
            SIEventsHandler.OnObjectMovement -= MoveObj;
            SIEventsHandler.OnEnemySpeedMultiplierChanged -= UpdateMovementStep;
        }

        public void MoveObj()
        {
            MoveObject(_moveStep, true);
        }

        private void UpdateMovementStep(float newStep)
        {
            _moveStep = newStep;
        }
    }
}
