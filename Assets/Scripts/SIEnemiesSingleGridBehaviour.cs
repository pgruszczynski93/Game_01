using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemiesSingleGridBehaviour : MonoBehaviour, IMoveable
    {
        [SerializeField] private GameObject[] _enemiesInGrid;

        [SerializeField] private SimpleTween2DInfo _enemyGridTweenInfo;

        private int _totalEnemies;
        private int _livingEnemies;
        private int _speedMultiplier;

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
        }

        private void OnDisable()
        {
            SIEventsHandler.OnEnemyDeath -= DecreaseEnemiesCount;
            SIEventsHandler.OnEnemyDeath -= UpdateCurrentSpeedMultiplier;
            SIEventsHandler.OnEnemyDeath -= CheckEnemyWaveEnd;
        }

        private void SetInitialReferences()
        {
            if (_enemiesInGrid == null || _enemyGridTweenInfo == null || _enemiesInGrid.Length == 0)
            {
                Debug.LogError("Enemies grid array fields aren't initialized.");
                return;
            }
            _totalEnemies = _enemiesInGrid.Length;
            _livingEnemies = _totalEnemies;
            _cachedTransform = transform;
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
                }, _enemyGridTweenInfo, () => { SIEnemiesGridsMaster.Instance.IsEnemyMovementAllowed = true; }));

            StopAllCoroutines();
        }
        
        public void ResetGrid()
        {
            _cachedTransform.position = SIEnemiesGridsMaster.Instance.GridInitialPosition;
        }
    }
}

