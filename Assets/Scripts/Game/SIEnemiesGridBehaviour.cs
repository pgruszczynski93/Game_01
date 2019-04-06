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

        private void UpdateShootingEnemies(int index)
        {
            SIEnemyShootBehaviour deathEnemy = _enemies[index].ShootBehaviour;
            bool isDeathEnemyShootable = IsDeathEnemyShootable(deathEnemy);

            _enemiesAbleToShoot.Remove(deathEnemy);

            // poprawić tego ifa tak by nie przepuszczał dalej gdy jest 2 rzad
            Debug.LogWarning("Pased " + (isDeathEnemyShootable == false || index < SIConstants.ENEMIES_IN_ROW));

            if (isDeathEnemyShootable == false || index < SIConstants.ENEMIES_IN_ROW)
            {
                return;
            }

            SIEnemyShootBehaviour newShootable = GetNextShootableEnemy(index);
            _enemiesAbleToShoot.Add(newShootable);
        }

        private SIEnemyShootBehaviour GetNextShootableEnemy(int index)
        {
            const int enemiesInRow = SIConstants.ENEMIES_IN_ROW;
            int killedEnemyRow = index / enemiesInRow;
            int firstVerticalNeighbour;
            int secondVerticalNeighbour;

            if (killedEnemyRow == 2)
            {
                firstVerticalNeighbour = index - enemiesInRow;
                secondVerticalNeighbour = firstVerticalNeighbour - enemiesInRow;
            }
            else
            {
                firstVerticalNeighbour = index + enemiesInRow;
                secondVerticalNeighbour = index - enemiesInRow;
            }

            Debug.LogWarning(string.Format("FIRST {0}, SECOND {1} ALIVE F  {2} ALIVE s {3} ", firstVerticalNeighbour,
                secondVerticalNeighbour, _enemies[firstVerticalNeighbour].IsEnemyAlive(),
                _enemies[secondVerticalNeighbour].IsEnemyAlive()));

            return _enemies[firstVerticalNeighbour].IsEnemyAlive()
                ? _enemies[firstVerticalNeighbour].ShootBehaviour
                : _enemies[secondVerticalNeighbour].ShootBehaviour;
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