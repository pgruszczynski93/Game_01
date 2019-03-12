using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemiesGridBehaviour : MonoBehaviour, IMoveable
    {
        private const float MOVEMENT_STEP_1 = 7f;
        private const float MOVEMENT_STEP_2 = 5f;

        [SerializeField] private int _enemiesInRow;
        [SerializeField] private VectorTweenInfo _enemyGridTweenInfo;
        [SerializeField] private GameObject[] _enemiesInGrid;
        [SerializeField] private List<SIEnemyShootBehaviour> _enemiesAbleToShoot;

        private bool _isShootingAvailableForWave;

        private int _totalEnemies;
        private int _livingEnemies;
        private int _speedMultiplier;

        private float _gridSize;
        private float _lastRefreshTime;
        private float _shotAbilityRefreshTime;
        [SerializeField] private float _shotTimeMinBreak;
        [SerializeField] private float _shotTimeMaxBreak;

        private Transform _cachedTransform;
        private Vector2 _raycastDirection;
        private Vector2 _raycastOffset;

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

            SIEventsHandler.OnSwitchShootableEnemy += UpdateAbleToShootEnemies;
            SIEventsHandler.OnWaveEnd += ResetEnemyGrid;

            SIEventsHandler.OnDebugInputHandling += Debug_ResetWave;

        }

        private void OnDisable()
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

        private void SetInitialReferences()
        {
            if (_enemiesInGrid == null || _enemyGridTweenInfo == null || _enemiesInGrid.Length == 0 || _enemiesAbleToShoot == null)
            {
                Debug.LogError("Enemies grid array fields aren't initialized.");
                return;
            }

            _gridSize = 0.5f;
            _enemiesInRow = 5;
            _shotTimeMinBreak = 0.4f;
            _shotTimeMaxBreak = 1f;
            _totalEnemies = _enemiesInGrid.Length;
            _livingEnemies = _totalEnemies;
            _cachedTransform = transform;
            _enemyGridTweenInfo.startValue = SIEnemiesGridsMaster.Instance.GridInitialPosition;
            _enemyGridTweenInfo.endValue = SIEnemiesGridsMaster.Instance.GridScenePosition;
        }

        private void GetEnemiesAbleToShoot()
        {
            // Note: first initialization is made in Unity Editor (drag and drop).
            _enemiesAbleToShoot.Clear();
            for (int i = _totalEnemies - 1; i >= _totalEnemies - _enemiesInRow; i--)
            {
                SIEnemyShootBehaviour enemyAbleToShoot = _enemiesInGrid[i].GetComponent<SIEnemyShootBehaviour>();
                if (enemyAbleToShoot == null)
                {
                    SIHelpers.SISimpleLogger(this, "Enemy has no SIEnemyShootBehaviour attached. ", SimpleLoggerTypes.Error);
                    return;
                }

                _enemiesAbleToShoot.Add(enemyAbleToShoot);
            }
        }

        public void ResetEnemyGrid()
        {
            SIHelpers.SISimpleLogger(this, "ResetEnemyGrid: Grid reset", SimpleLoggerTypes.Log);
            SIEnemiesGridsMaster.Instance.IsEnemyInGridMovementAllowed = false;

            GetEnemiesAbleToShoot();
            _livingEnemies = _totalEnemies;
            _cachedTransform.position = SIEnemiesGridsMaster.Instance.GridInitialPosition;
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

            float newMultiplier = _livingEnemies == 1 ? MOVEMENT_STEP_1 : MOVEMENT_STEP_2;

            SIEventsHandler.OnEnemySpeedMultiplierChanged?.Invoke(newMultiplier);
        }

        private void CheckEnemyWaveEnd()
        {
            if (_livingEnemies > 0)
            {
                return;
            }
            SIEventsHandler.OnWaveEnd?.Invoke();
        }

        private void Debug_ResetWave()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                SIHelpers.SISimpleLogger(this, "Debug_ResetWave()", SimpleLoggerTypes.Log);
                SIEventsHandler.OnWaveEnd?.Invoke();
            }
        }

        public void MoveObj()
        {
          //  StartCoroutine(GridInitialMovementRoutine());
        }

        private IEnumerator GridInitialMovementRoutine()
        {
            yield return StartCoroutine(SIHelpers.SimpleTween3D((newPosition) =>
                {
                    _cachedTransform.position = newPosition;
                }, _enemyGridTweenInfo, () => { SIEnemiesGridsMaster.Instance.EnableGridMovements(); }));

            StopCoroutine(GridInitialMovementRoutine());
        }
        
        public void ResetGrid()
        {
            _cachedTransform.position = SIEnemiesGridsMaster.Instance.GridInitialPosition;
        }

        private void UpdateAbleToShootEnemies(SIShootedEnemyInfo enemiesInfo)
        {
            //if (_enemiesAbleToShoot == null || _enemiesAbleToShoot.Count == 0)
            //{
            //    SIHelpers.SISimpleLogger(this, "UpdateAbleToShootEnemies() stopped.", SimpleLoggerTypes.Error);
            //    return;
            //}

            SIHelpers.SISimpleLogger(this, "UpdateAbleToShootEnemies() next enemy " + enemiesInfo.nextShootableEnemy, SimpleLoggerTypes.Log);
            if (_enemiesAbleToShoot.Contains(enemiesInfo.currentShootableEnemy))
            {
                _enemiesAbleToShoot.Remove(enemiesInfo.currentShootableEnemy);

                if (enemiesInfo.nextShootableEnemy != null)
                {
                    _enemiesAbleToShoot.Add(enemiesInfo.nextShootableEnemy);
                }
            }
        }

        public void ShootWithAbleEnemies()
        {
            for (int i = 0; i < _enemiesAbleToShoot.Count; i++)
            {
                _enemiesAbleToShoot[i].InvokeShoot();
            }
        }

        public void StartShooting()
        {
            StartCoroutine(EnemiesShootingRoutine());
        }

        public void StopShooting()
        {
            SIHelpers.SISimpleLogger(this, "StopShooting(): shooting stopped - wave resetting " , SimpleLoggerTypes.Log);
            StopCoroutine(EnemiesShootingRoutine());
        }

        private IEnumerator EnemiesShootingRoutine()
        {
            if (_enemiesAbleToShoot == null || _enemiesAbleToShoot.Count == 0)
            {
                Debug.Log("Can't setup enemies shooting routine");
                yield break;
            }

            int enemiesAbleToShootCount = _enemiesAbleToShoot.Count;
            int enemySelectedToShootIndex = 0;
            float timeToNextShoot = 0.0f;

            while (SIEnemiesGridsMaster.Instance.IsEnemyInGridMovementAllowed && enemiesAbleToShootCount > 0)
            {
                enemiesAbleToShootCount = _enemiesAbleToShoot.Count;
                enemySelectedToShootIndex = Random.Range(0, (enemiesAbleToShootCount > 0) ? enemiesAbleToShootCount - 1 : 0);
                timeToNextShoot = Random.Range(_shotTimeMinBreak, _shotTimeMaxBreak);
                if (enemySelectedToShootIndex >= 0)
                {
                    _enemiesAbleToShoot[enemySelectedToShootIndex].InvokeShoot();
                }
                yield return new WaitForSeconds(timeToNextShoot);
            }

            yield return null;

        }

        public void StopObj()
        {
            throw new System.NotImplementedException();
        }
    }
}

