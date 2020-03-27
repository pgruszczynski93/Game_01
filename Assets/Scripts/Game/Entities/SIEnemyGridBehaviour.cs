using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyGridBehaviour : MonoBehaviour
    {
        [SerializeField] int _enemiesInRow;
        [SerializeField] float _shotTimeMinBreak;
        [SerializeField] float _shotTimeMaxBreak;
        [SerializeField] VectorTweenInfo _enemyGridTweenInfo;
        [SerializeField] SIEnemyBehaviour[] _enemies;
        [SerializeField] List<SIEnemyShootBehaviour> _enemiesAbleToShoot;

        bool _isShootingAvailableForWave;

        int _totalEnemies;
        int _livingEnemies;
        int _speedMultiplier;

        float _lastRefreshTime;
        float _shotAbilityRefreshTime;

        Transform _thisTransform;

        protected void Awake()
        {
            Initialise();
        }

        void OnEnable()
        {
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        void AssignEvents()
        {
            SIEventsHandler.OnEnemyDeath += DecreaseEnemiesCount;
            SIEventsHandler.OnEnemyDeath += UpdateCurrentSpeedMultiplier;
            SIEventsHandler.OnEnemyDeath += CheckEnemyWaveEnd;
            SIEventsHandler.OnShootingEnemiesUpdate += UpdateShootingEnemies;
            SIEventsHandler.OnWaveEnd += ResetEnemyGrid;
            SIEventsHandler.OnDebugInputHandling += Debug_ResetWave;
        }

        void RemoveEvents()
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
            for (int i = 0; i < SIConstants.ENEMIES_TOTAL; i++) _enemies[i].enemyIndex = i;
        }

        void OnDestroy()
        {
            StopAllCoroutines();
        }

        void Initialise()
        {
            _enemiesInRow = SIConstants.ENEMIES_IN_ROW;
            _shotTimeMinBreak = SIConstants.ENEMY_MIN_SHOOT_DELAY;
            _shotTimeMaxBreak = SIConstants.ENEMY_MAX_SHOOT_DELAY;
            _totalEnemies = _enemies.Length;
            _livingEnemies = _totalEnemies;
            _thisTransform = transform;
//            _enemyGridTweenInfo.startValue = SIEnemiesGridManager.Instance.GridInitialPosition;
//            _enemyGridTweenInfo.endValue = SIEnemiesGridManager.Instance.GridScenePosition;
            _enemiesAbleToShoot = new List<SIEnemyShootBehaviour>();
            AssignEnemyIndexes();
            GetEnemiesAbleToShoot();
        }

        void GetEnemiesAbleToShoot()
        {
            _enemiesAbleToShoot.Clear();
            for (int i = _totalEnemies - 1; i >= _totalEnemies - _enemiesInRow; i--)
            {
                SIEnemyShootBehaviour enemyAbleToShoot = _enemies[i].ShootBehaviour;
                if (enemyAbleToShoot == null)
                    return;

                _enemiesAbleToShoot.Add(enemyAbleToShoot);
            }
        }

        public void ResetEnemyGrid()
        {
            SIHelpers.SISimpleLogger(this, "ResetEnemyGrid: Grid reset", SimpleLoggerTypes.Log);
//            SIEnemiesGridManager.Instance.IsEnemyGridMovementAllowed = false;
            _livingEnemies = _totalEnemies;
//            _thisTransform.position = SIEnemiesGridManager.Instance.GridInitialPosition;
            GetEnemiesAbleToShoot();
        }

        void DecreaseEnemiesCount()
        {
            --_livingEnemies;
        }

        void UpdateCurrentSpeedMultiplier()
        {
            if (_livingEnemies > SIConstants.ENEMIES_LEFT_TO_INCREASE_GRID_MOVEMENT_STEP)
                return;

            float newMultiplier = _livingEnemies == SIConstants.ENEMIES_LEFT_TO_INCREASE_GRID_MOVEMENT_STEP
                ? SIConstants.ENEMYGRID_MOVEMENT_STEP_1
                : SIConstants.ENEMYGRID_MOVEMENT_STEP_2;

            SIEventsHandler.BroadcastOnEnemySpeedMultiplierChanged(newMultiplier);
        }

        void CheckEnemyWaveEnd()
        {
            if (_livingEnemies > 0) return;

            StartCoroutine(SIWaitUtils.WaitAndInvoke(SIConstants.END_WAVE_DELAY,
                () => { SIEventsHandler.BroadcastOnWaveEnd(); }));
        }

        void Debug_ResetWave()
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

        IEnumerator GridInitialMovementRoutine()
        {
            yield return null;
//            SIEnemiesGridManager.Instance.EnableGridMovementsWithShooting();
//            yield return StartCoroutine(SIHelpers.SimpleTween3D(
//                (newPosition) => { _thisTransform.position = newPosition; }, _enemyGridTweenInfo,
//                () => { SIEnemiesGridsMaster.Instance.EnableGridMovementsWithShooting(); }));
        }

        public void ResetGrid()
        {
//            _thisTransform.position = SIEnemiesGridManager.Instance.GridInitialPosition;
        }

        void UpdateShootingEnemies(int index)
        {
            SIEnemyShootBehaviour deathEnemy = _enemies[index].ShootBehaviour;
            bool isDeathEnemyShootable = IsDeathEnemyShootable(deathEnemy);
            int killedEnemyRow = index / SIConstants.ENEMIES_IN_ROW;

            _enemiesAbleToShoot.Remove(deathEnemy);

            if (isDeathEnemyShootable == false || killedEnemyRow == 0) return;

            if (IsPossibleToChangeShootingEnemy(index, killedEnemyRow, out int firstVerticalNeighbour,
                    out int secondVerticalNeighbour) == false)
                return;

            SIEnemyShootBehaviour newShootable = GetNextShootableEnemy(firstVerticalNeighbour, secondVerticalNeighbour);
            _enemiesAbleToShoot.Add(newShootable);
        }

        bool IsPossibleToChangeShootingEnemy(int index, int killedEnemyRow, out int firstVerticalNeighbour,
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

        bool AreNeighboursDead(int indexOfFirst, int indexOfSecond)
        {
            return _enemies[indexOfFirst].IsEnemyAlive() == false && _enemies[indexOfSecond].IsEnemyAlive() == false;
        }

        SIEnemyShootBehaviour GetNextShootableEnemy(int indexOfFirst, int indexOfSecond)
        {
            return _enemies[indexOfFirst].IsEnemyAlive()
                ? _enemies[indexOfFirst].ShootBehaviour
                : _enemies[indexOfSecond].ShootBehaviour;
        }

        bool IsDeathEnemyShootable(SIEnemyShootBehaviour shootingEnemy)
        {
            return _enemiesAbleToShoot.Contains(shootingEnemy);
        }

        public void StartShooting()
        {
            StartCoroutine(EnemiesShootingRoutine());
        }

        public void StopShooting()
        {
            StopCoroutine(EnemiesShootingRoutine());
        }

        IEnumerator EnemiesShootingRoutine()
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

            while ( /*SIEnemiesGridManager.Instance.IsEnemyGridMovementAllowed &&*/ enemiesAbleToShootCount > 0)
            {
                enemiesAbleToShootCount = _enemiesAbleToShoot.Count;
                anyEnemyIsAlive = enemiesAbleToShootCount > 0;
                // shift rand value to be in (0, n-1) size lenght value
                enemySelectedToShootIndex = Random.Range(1, anyEnemyIsAlive ? enemiesAbleToShootCount + 1 : 1);
                timeToNextShoot = Random.Range(_shotTimeMinBreak, _shotTimeMaxBreak);
                if (anyEnemyIsAlive) _enemiesAbleToShoot[enemySelectedToShootIndex - 1].Shoot();

                yield return SIWaitUtils.WaitForCachedSeconds(timeToNextShoot);
            }
        }
    }
}