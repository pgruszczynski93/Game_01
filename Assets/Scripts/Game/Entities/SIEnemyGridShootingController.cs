using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyGridShootingController : MonoBehaviour
    {
        [SerializeField] GridShootingSetup _gridBehaviourSetup;
        
//        [SerializeField] SIEnemyBehaviour[] _enemies;
        [SerializeField] List<SIEnemyShootBehaviour> _enemiesAbleToShoot;
        
        bool _isShootingAvailableForWave;

        float _lastRefreshTime;
        float _shotAbilityRefreshTime;

        GridShootingSettings _gridBehaviourSettings;

        void Awake()
        {
            PreInitialise();
        }
        
        void PreInitialise()
        {
            _gridBehaviourSettings = _gridBehaviourSetup.shootingSettings;
            _enemiesAbleToShoot = new List<SIEnemyShootBehaviour>();
        }

        void Start()
        {
            Initialise();
        }
        void Initialise()
        {
            SelectStartShootingEnemies();
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
            SIEnemyGridEvents.OnSubscribeToShooting += HandleOnSubscribeToShooting;
            
//            
//            SIEventsHandler.OnShootingEnemiesUpdate += HandleOnShootingEnemiesUpdate;
//            SIEventsHandler.OnWaveEnd += HandleOnWaveEnd;
//            //todo: DONT REMOVE THIS: OnDebugInputHandling Event -> REFACTOR
//            SIEventsHandler.OnDebugInputHandling += Debug_ResetWave;
        }

        void RemoveEvents()
        {
            SIEnemyGridEvents.OnSubscribeToShooting -= HandleOnSubscribeToShooting;
//            
//            SIEventsHandler.OnShootingEnemiesUpdate -= HandleOnShootingEnemiesUpdate;
//            SIEventsHandler.OnWaveEnd -= HandleOnWaveEnd;
//            //todo: DONT REMOVE THIS: OnDebugInputHandling Event -> REFACTOR
//            SIEventsHandler.OnDebugInputHandling -= Debug_ResetWave;
        }
        
        void HandleOnSubscribeToShooting(SIEnemyShootBehaviour enemyShootBehaviour)
        {
            _enemiesAbleToShoot.Add(enemyShootBehaviour);
        }

        void SelectStartShootingEnemies()
        {
            List<SIEnemyShootBehaviour> initialShootBehaviours = new List<SIEnemyShootBehaviour>();
            SIEnemyShootBehaviour currentBehaviour;
            for (int i = 0; i < _enemiesAbleToShoot.Count; i++)
            {
                currentBehaviour = _enemiesAbleToShoot[i];
                if (currentBehaviour.ShootBehaviourSetup.enemyIndex < _gridBehaviourSettings.startShootingThresholdIndex)
                    continue;

                initialShootBehaviours.Add(currentBehaviour);
            }

            _enemiesAbleToShoot = initialShootBehaviours;
        }

//        void HandleOnShootingEnemiesUpdate(int index)
//        {
//            UpdateShootingEnemies(index);
//        }

//        void HandleOnWaveEnd()
//        {
//            ResetGridShooting();
//        }

//        void OnDestroy()
//        {
//            StopAllCoroutines();
//        }

//        void ResetGridShooting()
//        {
//            _livingEnemies = _totalEnemies;
//            InitialiseRowOfShootingEnemies();
//            
//            SIEnemyGridEvents.BroadcastOnGridReset();
//        }
//        


        void Debug_ResetWave()
        {
            if (Input.GetKeyDown(KeyCode.G) == false)
                return;

            SIEventsHandler.BroadcastOnWaveEnd();
        }

//        void UpdateShootingEnemies(int index)
//        {
//            SIEnemyShootBehaviour deathEnemy = _enemies[index].ShootBehaviour;
//            bool isDeathEnemyShootable = IsDeathEnemyShootable(deathEnemy);
//            int killedEnemyRow = index / SIConstants.ENEMIES_IN_ROW;
//
//            _enemiesAbleToShoot.Remove(deathEnemy);
//
//            if (isDeathEnemyShootable == false || killedEnemyRow == 0) return;
//
//            if (IsPossibleToChangeShootingEnemy(index, killedEnemyRow, out int firstVerticalNeighbour,
//                    out int secondVerticalNeighbour) == false)
//                return;
//
//            SIEnemyShootBehaviour newShootable = GetNextShootableEnemy(firstVerticalNeighbour, secondVerticalNeighbour);
//            _enemiesAbleToShoot.Add(newShootable);
//        }

//        bool IsPossibleToChangeShootingEnemy(int index, int killedEnemyRow, out int firstVerticalNeighbour,
//            out int secondVerticalNeighbour)
//        {
//            if (killedEnemyRow == 2)
//            {
//                firstVerticalNeighbour = index - SIConstants.ENEMIES_IN_ROW;
//                secondVerticalNeighbour = firstVerticalNeighbour - SIConstants.ENEMIES_IN_ROW;
//            }
//            else
//            {
//                firstVerticalNeighbour = index + SIConstants.ENEMIES_IN_ROW;
//                secondVerticalNeighbour = index - SIConstants.ENEMIES_IN_ROW;
//            }
//
//            return AreNeighboursDead(firstVerticalNeighbour, secondVerticalNeighbour) == false;
//        }

//        bool AreNeighboursDead(int indexOfFirst, int indexOfSecond)
//        {
//            return _enemies[indexOfFirst].IsEnemyAlive() == false && _enemies[indexOfSecond].IsEnemyAlive() == false;
//        }
//
//        SIEnemyShootBehaviour GetNextShootableEnemy(int indexOfFirst, int indexOfSecond)
//        {
//            return _enemies[indexOfFirst].IsEnemyAlive()
//                ? _enemies[indexOfFirst].ShootBehaviour
//                : _enemies[indexOfSecond].ShootBehaviour;
//        }

//        bool IsDeathEnemyShootable(SIEnemyShootBehaviour shootingEnemy)
//        {
//            return _enemiesAbleToShoot.Contains(shootingEnemy);
//        }
//
//        public void StartShooting()
//        {
//            StartCoroutine(EnemiesShootingRoutine());
//        }
//
//        public void StopShooting()
//        {
//            StopCoroutine(EnemiesShootingRoutine());
//        }
//
//        IEnumerator EnemiesShootingRoutine()
//        {
//            if (_enemiesAbleToShoot == null || _enemiesAbleToShoot.Count == 0)
//            {
//                Debug.Log("Can't setup enemies shooting routine");
//                yield break;
//            }
//
//            bool anyEnemyIsAlive;
//            int enemiesAbleToShootCount = _enemiesAbleToShoot.Count;
//            int enemySelectedToShootIndex = 0;
//            float timeToNextShoot = 0.0f;
//
//            while ( /*SIEnemiesGridManager.Instance.IsEnemyGridMovementAllowed &&*/ enemiesAbleToShootCount > 0)
//            {
//                enemiesAbleToShootCount = _enemiesAbleToShoot.Count;
//                anyEnemyIsAlive = enemiesAbleToShootCount > 0;
//                // shift rand value to be in (0, n-1) size lenght value
//                enemySelectedToShootIndex = Random.Range(1, anyEnemyIsAlive ? enemiesAbleToShootCount + 1 : 1);
//                timeToNextShoot = Random.Range(_gridBehaviourSettings.minShootingInterval, _gridBehaviourSettings.maxShootingInterval);
////                if (anyEnemyIsAlive) _enemiesAbleToShoot[enemySelectedToShootIndex - 1].Shoot();
//
//                yield return SIWaitUtils.WaitForCachedSeconds(timeToNextShoot);
//            }
//        }
    }
}