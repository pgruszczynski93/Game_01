using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceInvaders
{
    public class SIEnemiesGridShootController : MonoBehaviour
    {
        [SerializeField] bool _isGridShootingLockedByDev;

        [SerializeField] GridShootingSetup _gridBehaviourSetup;
        [SerializeField] List<SIEnemyShootController> _enemiesAbleToShoot;

        bool _initialised;
        bool _isGridShootingEnabled;
        int _totalEnemiesAbleToShoot;
        Coroutine _shootingRoutine;
        GridShootingSettings _gridBehaviourSettings;

        void TryInitialise() {
            if (_initialised)
                return;
            
            _initialised = true;
            _gridBehaviourSettings = _gridBehaviourSetup.shootingSettings;
            _enemiesAbleToShoot = new List<SIEnemyShootController>();
        }

        void OnDestroy() {
            if(_shootingRoutine != null)
                StopCoroutine(_shootingRoutine);
        }

        void OnEnable()
        {
            SubscribeEvents();
        }

        void OnDisable()
        {
            UnsubscribeEvents();
        }

        void SubscribeEvents()
        {
            SIEnemyGridEvents.OnGridReset += HandleOnGridReset;
            SIEnemyGridEvents.OnGridShootingReset += HandleOnGridShootingReset;
            SIEnemyGridEvents.OnReadyToShoot += HandleOnReadyToShoot;
            SIGameplayEvents.OnEnemyDeath += HandleOnEnemyDeath;
        }

        void UnsubscribeEvents()
        {
            SIEnemyGridEvents.OnGridReset -= HandleOnGridReset;
            SIEnemyGridEvents.OnGridShootingReset -= HandleOnGridShootingReset;
            SIEnemyGridEvents.OnReadyToShoot -= HandleOnReadyToShoot;
            SIGameplayEvents.OnEnemyDeath -= HandleOnEnemyDeath;
        }
        
        void HandleOnReadyToShoot(SIEnemyShootController enemyShootController) {
            TryInitialise();
            _enemiesAbleToShoot.Add(enemyShootController);
        }

        void EnableGridShootingPossibility(bool isGridShootingEnabled)
        {
            _isGridShootingEnabled = isGridShootingEnabled;
        }

        void HandleOnGridReset()
        {
            EnableGridShootingPossibility(false);
            ResetShootingEnemiesInstances();
        }

        void HandleOnGridShootingReset()
        {
            EnableGridShootingPossibility(true);
            TryToRunGridShootingRoutine();
        }

        void ResetShootingEnemiesInstances()
        {
            List<SIEnemyShootController> initialShootBehaviours = new List<SIEnemyShootController>();
            SIEnemyShootController currentController;
            for (int i = 0; i < _enemiesAbleToShoot.Count; i++)
            {
                currentController = _enemiesAbleToShoot[i];
                if (currentController.ShootBehaviourSetup.enemyIndex <
                    _gridBehaviourSettings.startShootingThresholdIndex)
                    continue;

                currentController.CanShoot = true;
                initialShootBehaviours.Add(currentController);
            }

            _enemiesAbleToShoot = initialShootBehaviours;
            SetEnemiesAbleToShootCount();
        }

        void SetEnemiesAbleToShootCount()
        {
            _totalEnemiesAbleToShoot = _enemiesAbleToShoot.Count;
        }

        void HandleOnEnemyDeath(SIEnemyBehaviour deadEnemy)
        {
            TryToUpdateShootingEnemies(deadEnemy.EnemyShootController);
        }

        void TryToUpdateShootingEnemies(SIEnemyShootController deadEnemyShootController)
        {
            if (!WasKilledEnemyAbleToShoot(deadEnemyShootController))
                return;

            _enemiesAbleToShoot.Remove(deadEnemyShootController);
            SetEnemiesAbleToShootCount();
        }

        bool WasKilledEnemyAbleToShoot(SIEnemyShootController deadEnemyShootController)
        {
            return _enemiesAbleToShoot.Contains(deadEnemyShootController);
        }

        bool ShouldStopGridShooting()
        {
            return _enemiesAbleToShoot == null || _enemiesAbleToShoot.Count == 0 || !_isGridShootingEnabled;
        }

        void TryToRunGridShootingRoutine()
        {
            if (ShouldStopGridShooting() || _isGridShootingLockedByDev)
                return;
            _shootingRoutine = StartCoroutine(GridShootingRoutine());
        }


        IEnumerator GridShootingRoutine()
        {
            int indexOfSelectedEnemy;
            
            while (_totalEnemiesAbleToShoot > 0)
            {
                indexOfSelectedEnemy = Random.Range(0, _totalEnemiesAbleToShoot);
                SIEnemyGridEvents.BroadcastOnShotInvoked(_enemiesAbleToShoot[indexOfSelectedEnemy]);
                yield return WaitUtils.WaitForCachedSeconds(Random.Range(
                    _gridBehaviourSettings.minShootingInterval, _gridBehaviourSettings.maxShootingInterval));
            }
        }
    }
}