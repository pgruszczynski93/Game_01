using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemiesGridBehaviour : MonoBehaviour
    {
        [SerializeField] private int _enemiesInRow;
        [SerializeField] private VectorTweenInfo _enemyGridTweenInfo;
        [SerializeField] private GameObject[] _enemiesInGrid;
        [SerializeField] private List<IShootable> _enemiesAbleToShoot;

        private bool _isShootingAvailableForWave;

        private int _totalEnemies;
        private int _livingEnemies;
        private int _speedMultiplier;

        private float _lastRefreshTime;
        private float _shotAbilityRefreshTime;
        [SerializeField] private float _shotTimeMinBreak;
        [SerializeField] private float _shotTimeMaxBreak;

        private Transform _cachedTransform;
        private Vector2 _raycastDirection;
        private Vector2 _raycastOffset;

        protected void Awake()
        {
            Initialize();
        }

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
            SIEventsHandler.OnEnemyDeath += DecreaseEnemiesCount;
            SIEventsHandler.OnEnemyDeath += UpdateCurrentSpeedMultiplier;
            SIEventsHandler.OnEnemyDeath += CheckEnemyWaveEnd;

            SIEventsHandler.OnSwitchShootableEnemy += UpdateAbleToShootEnemies;
            SIEventsHandler.OnWaveEnd += ResetEnemyGrid;

            SIEventsHandler.OnDebugInputHandling += Debug_ResetWave;
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnEnemyDeath -= DecreaseEnemiesCount;
            SIEventsHandler.OnEnemyDeath -= UpdateCurrentSpeedMultiplier;
            SIEventsHandler.OnEnemyDeath -= CheckEnemyWaveEnd;

            SIEventsHandler.OnSwitchShootableEnemy -= UpdateAbleToShootEnemies;
            SIEventsHandler.OnWaveEnd -= ResetEnemyGrid;

            SIEventsHandler.OnDebugInputHandling -= Debug_ResetWave;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void Initialize()
        {
            _enemiesInRow = SIConstants.ENEMIES_IN_ROW;
            _shotTimeMinBreak = SIConstants.ENEMY_MIN_SHOOT_DELAY;
            _shotTimeMaxBreak = SIConstants.ENEMY_MAX_SHOOT_DELAY;
            _totalEnemies = _enemiesInGrid.Length;
            _livingEnemies = _totalEnemies;
            _cachedTransform = transform;
            _enemyGridTweenInfo.startValue = SIEnemiesGridsMaster.Instance.GridInitialPosition;
            _enemyGridTweenInfo.endValue = SIEnemiesGridsMaster.Instance.GridScenePosition;
            _enemiesAbleToShoot = new List<IShootable>();
            GetEnemiesAbleToShoot();
        }

        private void GetEnemiesAbleToShoot()
        {
            _enemiesAbleToShoot.Clear();
            for (int i = _totalEnemies - 1; i >= _totalEnemies - _enemiesInRow; i--)
            {
                SIEnemyShootBehaviour enemyAbleToShoot = _enemiesInGrid[i].GetComponent<SIEnemyShootBehaviour>();
                if (enemyAbleToShoot == null)
                {
                    return;
                }

                _enemiesAbleToShoot.Add(enemyAbleToShoot);
            }
        }

        public void ResetEnemyGrid()
        {
            StartCoroutine(SIHelpers.CustomDelayRoutine(SIConstants.END_WAVE_DELAY * 10f));

            SIHelpers.SISimpleLogger(this, "ResetEnemyGrid: Grid reset", SimpleLoggerTypes.Log);
            SIEnemiesGridsMaster.Instance.IsEnemyInGridMovementAllowed = false;
            _livingEnemies = _totalEnemies;
            _cachedTransform.position = SIEnemiesGridsMaster.Instance.GridInitialPosition;
            GetEnemiesAbleToShoot();
        }

        private void DecreaseEnemiesCount()
        {
            --_livingEnemies;
        }

        private void UpdateCurrentSpeedMultiplier()
        {
            if (_livingEnemies > SIConstants.ENEMIES_LEFT_TO_INCREASE_GRID_MOVEMENT_STEP)
            {
                return;
            }

            float newMultiplier = _livingEnemies == SIConstants.ENEMIES_LEFT_TO_INCREASE_GRID_MOVEMENT_STEP
                ? SIConstants.ENEMYGRID_MOVEMENT_STEP_1
                : SIConstants.ENEMYGRID_MOVEMENT_STEP_2;

            SIEventsHandler.BroadcastOnEnemySpeedMultiplierChanged(newMultiplier);
        }

        private void CheckEnemyWaveEnd()
        {
            if (_livingEnemies > 0)
            {
                return;
            }

            SIEventsHandler.BroadcastOnWaveEnd();
        }

        private void Debug_ResetWave()
        {
            if (Input.GetKeyDown(KeyCode.G) == false) 
                return;
            
            SIHelpers.SISimpleLogger(this, "Debug_ResetWave()", SimpleLoggerTypes.Log);
            SIEventsHandler.BroadcastOnWaveEnd();
        }

        public void MoveEnemiesGrid()
        {
            StartCoroutine(GridInitialMovementRoutine());
        }

        private IEnumerator GridInitialMovementRoutine()
        {
            yield return StartCoroutine(SIHelpers.SimpleTween3D(
                (newPosition) => { _cachedTransform.position = newPosition; }, _enemyGridTweenInfo,
                () => { SIEnemiesGridsMaster.Instance.EnableGridMovementsWithShooting(); }));

            StopCoroutine(GridInitialMovementRoutine());
        }

        public void ResetGrid()
        {
            _cachedTransform.position = SIEnemiesGridsMaster.Instance.GridInitialPosition;
        }

        private void UpdateAbleToShootEnemies(SIShootedEnemyInfo enemiesInfo)
        {
            if (_enemiesAbleToShoot.Contains(enemiesInfo.currentShootableEnemy) == false)
            {
                return;
            }

            _enemiesAbleToShoot.Remove(enemiesInfo.currentShootableEnemy);

            if (enemiesInfo.nextShootableEnemy != null)
            {
                _enemiesAbleToShoot.Add(enemiesInfo.nextShootableEnemy);
            }
        }

        public void ShootWithAbleEnemies()
        {
            for (int i = 0; i < _enemiesAbleToShoot.Count; i++)
            {
                _enemiesAbleToShoot[i].Shoot();
            }
        }

        public void StartShooting()
        {
            StartCoroutine(EnemiesShootingRoutine());
        }

        public void StopShooting()
        {
            SIHelpers.SISimpleLogger(this, "StopShooting(): shooting stopped - wave resetting ", SimpleLoggerTypes.Log);
            StopCoroutine(EnemiesShootingRoutine());
        }

        private IEnumerator EnemiesShootingRoutine()
        {
            if (_enemiesAbleToShoot == null || _enemiesAbleToShoot.Count == 0)
            {
                Debug.Log("Can't setup enemies shooting routine");
                yield break;
            }

            bool anyEnemyIsAlive;
            int enemiesAbleToShootCount = _enemiesAbleToShoot.Count;
            int enemySelectedToShootIndex = 0;
            float timeToNextShoot = 0.0f;

            while (SIEnemiesGridsMaster.Instance.IsEnemyInGridMovementAllowed && enemiesAbleToShootCount > 0)
            {
                enemiesAbleToShootCount = _enemiesAbleToShoot.Count;
                anyEnemyIsAlive = enemiesAbleToShootCount > 0;
                enemySelectedToShootIndex = Random.Range(0, (anyEnemyIsAlive) ? enemiesAbleToShootCount - 1 : 0);
                timeToNextShoot = Random.Range(_shotTimeMinBreak, _shotTimeMaxBreak);
                if (enemySelectedToShootIndex >= 0)
                {
                    _enemiesAbleToShoot[enemySelectedToShootIndex].Shoot();
                }

                yield return SIHelpers.GetWFSCachedValue(timeToNextShoot);
            }
        }
    }
}