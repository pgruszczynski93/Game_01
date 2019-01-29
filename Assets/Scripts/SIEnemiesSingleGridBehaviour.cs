using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemiesSingleGridBehaviour : MonoBehaviour, IMoveable
    {
        [SerializeField] private GameObject[] _enemiesInGrid;
        [SerializeField] private SimpleTween2DInfo _enemyGridTweenInfo;
        [SerializeField] private GameObject[] _enemiesAbleToShoot;

        private int _totalEnemies;
        private int _livingEnemies;
        private int _speedMultiplier;
        private int _enemiesInRow;
        private float _gridSize;

        private Transform _cachedTransform;

        public GameObject[] EnemiesInGrid
        {
            get { return _enemiesInGrid; }
        }

        protected void Awake()
        {
            SetInitialReferences();
        }

        private void OnEnable()
        {
            SIEventsHandler.OnEnemyDeath += DecreaseEnemiesCount;
            SIEventsHandler.OnEnemyDeath += UpdateCurrentSpeedMultiplier;
            SIEventsHandler.OnEnemyDeath += CheckEnemyWaveEnd;

            SIEventsHandler.OnObjectMovement += GetEnemiesAbleToShoot;
        }

        private void OnDisable()
        {
            SIEventsHandler.OnEnemyDeath -= DecreaseEnemiesCount;
            SIEventsHandler.OnEnemyDeath -= UpdateCurrentSpeedMultiplier;
            SIEventsHandler.OnEnemyDeath -= CheckEnemyWaveEnd;

            SIEventsHandler.OnObjectMovement += GetEnemiesAbleToShoot;
        }


        private void SetInitialReferences()
        {
            if (_enemiesInGrid == null || _enemyGridTweenInfo == null || _enemiesInGrid.Length == 0)
            {
                Debug.LogError("Enemies grid array fields aren't initialized.");
                return;
            }

            _enemiesInRow = 11;
            _totalEnemies = _enemiesInGrid.Length;
            _livingEnemies = _totalEnemies;
            _cachedTransform = transform;
            _enemiesAbleToShoot = new GameObject[_enemiesInRow];
            _enemyGridTweenInfo.startPos = SIEnemiesGridsMaster.Instance.GridInitialPosition;
            _enemyGridTweenInfo.endPos = SIEnemiesGridsMaster.Instance.GridScenePosition;
        }

        private void DecreaseEnemiesCount()
        {
            --_livingEnemies;
        }

        private void UpdateCurrentSpeedMultiplier()
        {
            if (_livingEnemies > 2)
            {
                return;
            }

            float newMultiplier = _livingEnemies == 1 ? 6.0f : 4.0f;

            SIEventsHandler.OnEnemySpeedMultiplierChanged?.Invoke(newMultiplier);
        }

        private void CheckEnemyWaveEnd()
        {
            if (_livingEnemies > 0)
            {
                return;
            }
            Debug.Log("WAVE END ");
            //SIEventsHandler.OnWaveEnd?.Invoke();
        }

        public void MoveObj()
        {
            StartCoroutine(GridInitialMovementRoutine());
        }

        private IEnumerator GridInitialMovementRoutine()
        {
            yield return StartCoroutine(SIHelpers.SimpleTween3D((newPosition) =>
                {
                    _cachedTransform.position = newPosition;
                }, _enemyGridTweenInfo, () => { SIEnemiesGridsMaster.Instance.EnableGridMovements(); }));

            StopAllCoroutines();
        }
        
        public void ResetGrid()
        {
            _cachedTransform.position = SIEnemiesGridsMaster.Instance.GridInitialPosition;
        }

        private void GetEnemiesAbleToShoot()
        {
            Vector2 verticalNeighbourPosition;

            for (int i = 0; i < _totalEnemies; i++)
            {

            }
        }
    }
}

