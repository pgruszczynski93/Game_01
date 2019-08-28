using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemiesGridsMaster : SIGenericSingleton<SIEnemiesGridsMaster>
    {
        [SerializeField] private bool _isEnemyInGridMovementAllowed;

        [SerializeField] private Transform _gridInitialTrasnform;
        [SerializeField] private Transform _gridSceneTransform;

        [SerializeField] private SIEnemyGridBehaviour enemyGridBehaviour;

        public bool IsEnemyInGridMovementAllowed
        {
            get { return _isEnemyInGridMovementAllowed; }
            set { _isEnemyInGridMovementAllowed = value; }
        }

        public Vector3 GridInitialPosition => _gridInitialTrasnform.localPosition;
        public Vector3 GridScenePosition => _gridSceneTransform.localPosition;

        private void OnEnable()
        {
            AssignEvents();
        }

        private void OnDisable()
        {
            RemoveEvents();
        }

        private void AssignEvents()
        {
            SIEventsHandler.OnGameStarted += MoveEnemiesGrid;

            SIEventsHandler.OnWaveEnd += DisableGridMovementsWithShooting;
            SIEventsHandler.OnWaveEnd += MoveEnemiesGridWithDelay;
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnGameStarted -= MoveEnemiesGrid;

            SIEventsHandler.OnWaveEnd -= DisableGridMovementsWithShooting;
            SIEventsHandler.OnWaveEnd -= MoveEnemiesGridWithDelay;
        }

        private void MoveEnemiesGrid()
        {
            enemyGridBehaviour.MoveEnemiesGrid();
        }

        private void MoveEnemiesGridWithDelay()
        {
            StartCoroutine(SIWaitUtils.WaitAndInvoke(SIConstants.NEW_WAVE_COOLDOWN, MoveEnemiesGrid));
        }

        public void EnableGridMovementsWithShooting()
        {
            _isEnemyInGridMovementAllowed = true;
            enemyGridBehaviour.StartShooting();
        }

        public void DisableGridMovementsWithShooting()
        {
            _isEnemyInGridMovementAllowed = false;
            enemyGridBehaviour.StopShooting();
        }
    }
}