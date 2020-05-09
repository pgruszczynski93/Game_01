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
            SIEnemyGridEvents.OnGridReset += HandleOnGridReset;
            SIEnemyGridEvents.OnSubscribeToShooting += HandleOnSubscribeToShooting;
            SIEventsHandler.OnEnemyDeath += HandleOnEnemyDeath;
//            
//            SIEventsHandler.OnShootingEnemiesUpdate += HandleOnShootingEnemiesUpdate;
//            SIEventsHandler.OnWaveEnd += HandleOnWaveEnd;
//            //todo: DONT REMOVE THIS: OnDebugInputHandling Event -> REFACTOR
//            SIEventsHandler.OnDebugInputHandling += Debug_ResetWave;
        }

        void RemoveEvents()
        {
            SIEnemyGridEvents.OnGridReset -= HandleOnGridReset;
            SIEnemyGridEvents.OnSubscribeToShooting -= HandleOnSubscribeToShooting;
            SIEventsHandler.OnEnemyDeath -= HandleOnEnemyDeath;

//            SIEventsHandler.OnShootingEnemiesUpdate -= HandleOnShootingEnemiesUpdate;
//            SIEventsHandler.OnWaveEnd -= HandleOnWaveEnd;
//            //todo: DONT REMOVE THIS: OnDebugInputHandling Event -> REFACTOR
//            SIEventsHandler.OnDebugInputHandling -= Debug_ResetWave;
        }

        void HandleOnGridReset()
        {
            RestartShootingEnemies();
        }

        void HandleOnSubscribeToShooting(SIEnemyShootBehaviour enemyShootBehaviour)
        {
            _enemiesAbleToShoot.Add(enemyShootBehaviour);
        }

        void RestartShootingEnemies()
        {
            List<SIEnemyShootBehaviour> initialShootBehaviours = new List<SIEnemyShootBehaviour>();
            SIEnemyShootBehaviour currentBehaviour;
            for (int i = 0; i < _enemiesAbleToShoot.Count; i++)
            {
                currentBehaviour = _enemiesAbleToShoot[i];
                if (currentBehaviour.ShootBehaviourSetup.enemyIndex <
                    _gridBehaviourSettings.startShootingThresholdIndex)
                    continue;

                currentBehaviour.CanShoot = true;
                initialShootBehaviours.Add(currentBehaviour);
            }

            _enemiesAbleToShoot = initialShootBehaviours;
        }

        void HandleOnEnemyDeath(SIEnemyBehaviour deadEnemy)
        {
            TryToUpdateShootingEnemies(deadEnemy.ShootBehaviour);
        }

        void TryToUpdateShootingEnemies(SIEnemyShootBehaviour deadEnemyShootBehaviour)
        {
            if (!WasKilledEnemyAbleToShoot(deadEnemyShootBehaviour))
                return;

            _enemiesAbleToShoot.Remove(deadEnemyShootBehaviour);
        }

        bool WasKilledEnemyAbleToShoot(SIEnemyShootBehaviour deadEnemyShootBehaviour)
        {
            return _enemiesAbleToShoot.Contains(deadEnemyShootBehaviour);
        }

        void Debug_ResetWave()
        {
            if (Input.GetKeyDown(KeyCode.G) == false)
                return;

            SIEventsHandler.BroadcastOnWaveEnd();
        }
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