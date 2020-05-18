using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

namespace SpaceInvaders
{
    public class SIEnemyGridShootingController : MonoBehaviour
    {
        [SerializeField] GridShootingSetup _gridBehaviourSetup;
        [SerializeField] List<SIEnemyShootBehaviour> _enemiesAbleToShoot;

        bool _isGridShootingEnabled;
        int _totalEnemiesAbleToShoot;
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
            SIEnemyGridEvents.OnGridShootingReset += HandleOnGridShootingReset;
            SIEnemyGridEvents.OnSubscribeToShooting += HandleOnSubscribeToShooting;
            SIEventsHandler.OnEnemyDeath += HandleOnEnemyDeath;
        }

        void RemoveEvents()
        {
            SIEnemyGridEvents.OnGridReset -= HandleOnGridReset;
            SIEnemyGridEvents.OnGridShootingReset -= HandleOnGridShootingReset;
            SIEnemyGridEvents.OnSubscribeToShooting -= HandleOnSubscribeToShooting;
            SIEventsHandler.OnEnemyDeath -= HandleOnEnemyDeath;
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

        void HandleOnSubscribeToShooting(SIEnemyShootBehaviour enemyShootBehaviour)
        {
            _enemiesAbleToShoot.Add(enemyShootBehaviour);
        }

        void ResetShootingEnemiesInstances()
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
            SetEnemiesAbleToShootCount();
        }

        void SetEnemiesAbleToShootCount()
        {
            _totalEnemiesAbleToShoot = _enemiesAbleToShoot.Count;
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
            SetEnemiesAbleToShootCount();
        }

        bool WasKilledEnemyAbleToShoot(SIEnemyShootBehaviour deadEnemyShootBehaviour)
        {
            return _enemiesAbleToShoot.Contains(deadEnemyShootBehaviour);
        }

        bool ShouldStopGridShooting()
        {
            return _enemiesAbleToShoot == null || _enemiesAbleToShoot.Count == 0 || !_isGridShootingEnabled;
        }

        void TryToRunGridShootingRoutine()
        {
            if (ShouldStopGridShooting())
                return;
            StartCoroutine(GridShootingRoutine());
        }


        IEnumerator GridShootingRoutine()
        {
            int indexOfSelectedEnemy;
            
            while (_totalEnemiesAbleToShoot > 0)
            {
                indexOfSelectedEnemy = Random.Range(0, _totalEnemiesAbleToShoot);
                SIEnemyGridEvents.BroadcastOnShootOrderReceived(_enemiesAbleToShoot[indexOfSelectedEnemy]);
                yield return SIWaitUtils.WaitForCachedSeconds(Random.Range(
                    _gridBehaviourSettings.minShootingInterval, _gridBehaviourSettings.maxShootingInterval));
            }
        }
    }
}