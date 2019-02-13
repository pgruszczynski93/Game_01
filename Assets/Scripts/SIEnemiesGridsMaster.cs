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

        [SerializeField] private GameObject _enemiesGrid_1;

        [SerializeField] private SIEnemiesSingleGridBehaviour _enemiesGridBehaviour;


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
            SIEventsHandler.OnGameStarted += MoveEnemiesWave;
            SIEventsHandler.OnWaveEnd += DisableGridMovements;
        }

        private void OnDisable()
        {
            SIEventsHandler.OnGameStarted -= MoveEnemiesWave;
            SIEventsHandler.OnWaveEnd -= DisableGridMovements;
        }

        private void MoveEnemiesWave()
        {
            _enemiesGridBehaviour.MoveObj();
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



        //protected override void Awake()
        //{

        //}

        //private void SetInitialReferences()
        //{
        //}
    }
}
