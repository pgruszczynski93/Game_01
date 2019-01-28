using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemiesGridsMaster : SIGenericSingleton<SIEnemiesGridsMaster>
    {
        [SerializeField] private bool _isEnemyMovementAllowed;

        [SerializeField] private Transform _gridInitialTrasnform;
        [SerializeField] private Transform _gridSceneTransform;

        [SerializeField] private GameObject _enemiesGrid_1;
        [SerializeField] private GameObject _enemiesGrid_2;

        [SerializeField] private SIEnemiesSingleGridBehaviour _enemiesGridBehaviour_1;
        [SerializeField] private SIEnemiesSingleGridBehaviour _enemiesGridBehaviour_2;


        public bool IsEnemyMovementAllowed
        {
            get { return _isEnemyMovementAllowed; }
            set { _isEnemyMovementAllowed = value; }
        }

        public Transform GridInitialTrasnform { get => _gridInitialTrasnform; set => _gridInitialTrasnform = value; }
        public Transform GridSceneTrasnform { get => _gridSceneTransform; set => _gridSceneTransform = value; }

        public Vector3 GridInitialPosition { get => GridInitialTrasnform.localPosition; }
        public Vector3 GridScenePosition { get => GridSceneTrasnform.localPosition; }

        private void OnEnable()
        {
            SIEventsHandler.OnGameStarted += MoveEnemiesWave;
        }

        private void OnDisable()
        {
            SIEventsHandler.OnGameStarted += MoveEnemiesWave;
        }

        private void MoveEnemiesWave()
        {
            _enemiesGridBehaviour_1.MoveObj();
        }

        //protected override void Awake()
        //{

        //}

        //private void SetInitialReferences()
        //{
        //}
    }
}
