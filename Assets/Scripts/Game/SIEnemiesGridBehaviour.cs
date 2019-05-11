using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemiesGridBehaviour : MonoBehaviour
    {
        [SerializeField] private int _enemiesInRow;
        [SerializeField] private VectorTweenInfo _enemyGridTweenInfo;
        [SerializeField] private SIEnemyBehaviour[] _enemies;
        [SerializeField] private List<SIEnemyShootBehaviour> _enemiesAbleToShoot;

        private bool _isShootingAvailableForWave;

        private int _totalEnemies;
        private int _livingEnemies;
        private int _speedMultiplier;

        private float _lastRefreshTime;
        private float _shotAbilityRefreshTime;
        [SerializeField] private float _shotTimeMinBreak;
        [SerializeField] private float _shotTimeMaxBreak;

        private Transform _cachedTransform;

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

            SIEventsHandler.OnShootingEnemiesUpdate += UpdateShootingEnemies;

            SIEventsHandler.OnWaveEnd += ResetEnemyGrid;

            SIEventsHandler.OnDebugInputHandling += Debug_ResetWave;
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnEnemyDeath -= DecreaseEnemiesCount;
            SIEventsHandler.OnEnemyDeath -= UpdateCurrentSpeedMultiplier;
            SIEventsHandler.OnEnemyDeath -= CheckEnemyWaveEnd;
            SIEventsHandler.OnShootingEnemiesUpdate -= UpdateShootingEnemies;

            SIEventsHandler.OnWaveEnd -= ResetEnemyGrid;

            SIEventsHandler.OnDebugInputHandling -= Debug_ResetWave;
        }

        void AssignEnemyIndexes()
        {
            for (int i = 0; i < SIConstants.ENEMIES_TOTAL; i++)
            {
                _enemies[i].enemyIndex = i;
            }
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
            _totalEnemies = _enemies.Length;
            _livingEnemies = _totalEnemies;
            _cachedTransform = transform;
            _enemyGridTweenInfo.startValue = SIEnemiesGridsMaster.Instance.GridInitialPosition;
            _enemyGridTweenInfo.endValue = SIEnemiesGridsMaster.Instance.GridScenePosition;
            _enemiesAbleToShoot = new List<SIEnemyShootBehaviour>();
            AssignEnemyIndexes();
            GetEnemiesAbleToShoot();
        }

        private void GetEnemiesAbleToShoot()
        {
            _enemiesAbleToShoot.Clear();
            for (int i = _totalEnemies - 1; i >= _totalEnemies - _enemiesInRow; i--)
            {
                SIEnemyShootBehaviour enemyAbleToShoot = _enemies[i].ShootBehaviour;
                if (enemyAbleToShoot == null)
                {
                    return;
                }

                _enemiesAbleToShoot.Add(enemyAbleToShoot);
            }
        }

        public void ResetEnemyGrid()
        {
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

            StartCoroutine(SIHelpers.CustomDelayRoutine(SIConstants.END_WAVE_DELAY, () =>
            {
                SIEventsHandler.BroadcastOnWaveEnd();
            }));

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

        private void UpdateShootingEnemies(int index)
        {
            SIEnemyShootBehaviour deathEnemy = _enemies[index].ShootBehaviour;
            bool isDeathEnemyShootable = IsDeathEnemyShootable(deathEnemy);
            int killedEnemyRow = index / SIConstants.ENEMIES_IN_ROW;
            
            Debug.LogWarning("KILLED " + index);

            _enemiesAbleToShoot.Remove(deathEnemy);

            if (isDeathEnemyShootable == false || killedEnemyRow == 0)
            {
                return;
            }

            if (IsPossibleToChangeShootingEnemy(index, killedEnemyRow, out int firstVerticalNeighbour,
                out int secondVerticalNeighbour) == false)
            {
                return;
            }

            SIEnemyShootBehaviour newShootable = GetNextShootableEnemy(firstVerticalNeighbour, secondVerticalNeighbour);
            _enemiesAbleToShoot.Add(newShootable);
        }

        private bool IsPossibleToChangeShootingEnemy(int index, int killedEnemyRow, out int firstVerticalNeighbour,
            out int secondVerticalNeighbour)
        {
            if (killedEnemyRow == 2)
            {
                firstVerticalNeighbour = index - SIConstants.ENEMIES_IN_ROW;
                secondVerticalNeighbour = firstVerticalNeighbour - SIConstants.ENEMIES_IN_ROW;

            }
            else
            {
                firstVerticalNeighbour = index + SIConstants.ENEMIES_IN_ROW;
                secondVerticalNeighbour = index - SIConstants.ENEMIES_IN_ROW;
            }
            
            return AreNeighboursDead(firstVerticalNeighbour, secondVerticalNeighbour) == false;
        }

        private bool AreNeighboursDead(int indexOfFirst, int indexOfSecond)
        {
            return _enemies[indexOfFirst].IsEnemyAlive() == false && _enemies[indexOfSecond].IsEnemyAlive() == false;
        }

        private SIEnemyShootBehaviour GetNextShootableEnemy(int indexOfFirst, int indexOfSecond)
        {
            return _enemies[indexOfFirst].IsEnemyAlive()
                ? _enemies[indexOfFirst].ShootBehaviour
                : _enemies[indexOfSecond].ShootBehaviour;
        }

        private bool IsDeathEnemyShootable(SIEnemyShootBehaviour shootingEnemy)
        {
            return _enemiesAbleToShoot.Contains(shootingEnemy);
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
                // shift rand value to be in (0, n-1) size lenght value
                enemySelectedToShootIndex = Random.Range(1, (anyEnemyIsAlive) ? enemiesAbleToShootCount + 1 : 0);
                timeToNextShoot = Random.Range(_shotTimeMinBreak, _shotTimeMaxBreak);
                if (enemySelectedToShootIndex >= 0)
                {
                    _enemiesAbleToShoot[enemySelectedToShootIndex-1].Shoot();
                }

                yield return SIHelpers.GetWFSCachedValue(timeToNextShoot);
            }
        }
    }
}