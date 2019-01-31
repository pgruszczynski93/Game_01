using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyMovement : SIMovement, IMoveable
    {
        private float _moveStep;

        private bool _canEnemyMove;

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();
            _moveStep = 2;
        }

        protected override void OnEnable()
        {
            SIEventsHandler.OnObjectMovement += MoveObj;
            SIEventsHandler.OnEnemySpeedMultiplierChanged += UpdateMovementStep;
            SIEventsHandler.OnWaveEnd += ResetEnemy;
        }

        protected override void OnDisable()
        {
            SIEventsHandler.OnObjectMovement -= MoveObj;
            SIEventsHandler.OnEnemySpeedMultiplierChanged -= UpdateMovementStep;
            SIEventsHandler.OnWaveEnd -= ResetEnemy;
        }

        public void MoveObj()
        {
            if (SIEnemiesGridsMaster.Instance.IsEnemyInGridMovementAllowed == false)
            {
                return;
            }

            MoveObject(_moveStep, true);
        }

        private void UpdateMovementStep(float newStep)
        {
            _moveStep = newStep;
        }

        private void ResetEnemy()
        {
            Debug.Log("Reset EnemyPos");
            _cachedTransform.position = _startPosition;
        }

    }
}
