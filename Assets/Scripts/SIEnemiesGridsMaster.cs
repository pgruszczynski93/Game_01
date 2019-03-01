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

        [SerializeField] private SIEnemiesGridBehaviour _enemiesGridBehaviour;

        public bool IsEnemyInGridMovementAllowed
        {
            get { return _isEnemyInGridMovementAllowed; }
            set { _isEnemyInGridMovementAllowed = value; }
        }

        public Transform GridInitialTrasnform { get => _gridInitialTrasnform; set => _gridInitialTrasnform = value; }
        public Transform GridSceneTrasnform { get => _gridSceneTransform; set => _gridSceneTransform = value; }

        public Vector3 GridInitialPosition { get => GridInitialTrasnform.localPosition; }
        public Vector3 GridScenePosition { get => GridSceneTrasnform.localPosition; }

        private void OnEnable()
        {
            SIEventsHandler.OnGameStarted += MoveEnemiesGrid;

            SIEventsHandler.OnWaveEnd += DisableGridMovements;
            SIEventsHandler.OnWaveEnd += MoveEnemiesGridWithDelay;
        }

        private void OnDisable()
        {
            SIEventsHandler.OnGameStarted -= MoveEnemiesGrid;

            SIEventsHandler.OnWaveEnd -= DisableGridMovements;
            SIEventsHandler.OnWaveEnd -= MoveEnemiesGridWithDelay;
        }

        private void MoveEnemiesGrid()
        {
            _enemiesGridBehaviour.MoveObj();
        }

        private void MoveEnemiesGridWithDelay()
        {
            //sprawdzic to
            StartCoroutine(SIHelpers.CustomDelayRoutine(3, () =>
            {
                MoveEnemiesGrid();
            }));
        }

        public void EnableGridMovements()
        {
            _isEnemyInGridMovementAllowed = true;
            _enemiesGridBehaviour.StartShooting();
        }

        public void DisableGridMovements()
        {
            _isEnemyInGridMovementAllowed = false;
            _enemiesGridBehaviour.StopShooting();
        }

    }
}
