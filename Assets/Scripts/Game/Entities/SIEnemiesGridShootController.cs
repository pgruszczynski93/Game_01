using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders {
    public class SIEnemiesGridShootController : MonoBehaviour {
        [SerializeField] bool _isGridShootingLockedByDev;

        [SerializeField] GridShootingSetup _gridBehaviourSetup;
        [SerializeField] List<SIEnemyShootController> _enemiesAbleToShoot;

        bool _isGridShootingEnabled;
        int _totalEnemiesAbleToShoot;
        Coroutine _shootingRoutine;
        GridShootingSettings _gridBehaviourSettings;

        void Start() {
            Initialise();
        }

        void Initialise() {
            _gridBehaviourSettings = _gridBehaviourSetup.shootingSettings;
            _enemiesAbleToShoot = new List<SIEnemyShootController>();
        }

        void OnDestroy() {
            if (_shootingRoutine != null)
                StopCoroutine(_shootingRoutine);
        }

        void OnEnable() {
            SubscribeEvents();
        }

        void OnDisable() {
            UnsubscribeEvents();
        }

        void SubscribeEvents() {
            SIGameplayEvents.OnWaveStart += HandleOnWaveStart;
            SIGameplayEvents.OnWaveEnd += HandleOnWaveEnd;
            SIGameplayEvents.OnWaveCoolDown += HandleOnWaveCoolDown;
            SIEnemyGridEvents.OnReadyToShoot += HandleOnReadyToShoot;
            SIGameplayEvents.OnEnemyDeath += HandleOnEnemyDeath;
        }

        void UnsubscribeEvents() {
            SIGameplayEvents.OnWaveStart -= HandleOnWaveStart;
            SIGameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
            SIGameplayEvents.OnWaveCoolDown += HandleOnWaveCoolDown;
            SIEnemyGridEvents.OnReadyToShoot -= HandleOnReadyToShoot;
            SIGameplayEvents.OnEnemyDeath -= HandleOnEnemyDeath;
        }

        void HandleOnReadyToShoot(SIEnemyShootController enemyShootController) {
            if (!_enemiesAbleToShoot.Contains(enemyShootController))
                _enemiesAbleToShoot.Add(enemyShootController);
        }

        void EnableGridShootingPossibility(bool isGridShootingEnabled) {
            _isGridShootingEnabled = isGridShootingEnabled;
        }

        void HandleOnWaveStart() {
            EnableGridShootingPossibility(true);
            TryToRunGridShootingRoutine();
        }

        void HandleOnWaveEnd() {
            EnableGridShootingPossibility(false);
        }

        void HandleOnWaveCoolDown() {
            SetEnemiesToShootOnWaveStart();
        }

        void SetEnemiesToShootOnWaveStart() {
            List<SIEnemyShootController> initialShootBehaviours = new List<SIEnemyShootController>();
            SIEnemyShootController currentController;
            for (int i = 0; i < _enemiesAbleToShoot.Count; i++) {
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

        void SetEnemiesAbleToShootCount() {
            _totalEnemiesAbleToShoot = _enemiesAbleToShoot.Count;
        }

        void HandleOnEnemyDeath(SIEnemyBehaviour deadEnemy) {
            TryToUpdateShootingEnemies(deadEnemy.EnemyShootController);
        }

        void TryToUpdateShootingEnemies(SIEnemyShootController deadEnemyShootController) {
            if (!EnemyAbleToShootKilled(deadEnemyShootController))
                return;

            _enemiesAbleToShoot.Remove(deadEnemyShootController);
            SetEnemiesAbleToShootCount();
        }

        bool EnemyAbleToShootKilled(SIEnemyShootController deadEnemyShootController) {
            return _enemiesAbleToShoot.Contains(deadEnemyShootController);
        }

        bool ShouldStopGridShooting() {
            return _enemiesAbleToShoot == null || _enemiesAbleToShoot.Count == 0 || !_isGridShootingEnabled;
        }

        void TryToRunGridShootingRoutine() {
            if (ShouldStopGridShooting() || _isGridShootingLockedByDev)
                return;
            _shootingRoutine = StartCoroutine(GridShootingRoutine());
        }


        IEnumerator GridShootingRoutine() {
            int indexOfSelectedEnemy;

            while (_totalEnemiesAbleToShoot > 0) {
                indexOfSelectedEnemy = Random.Range(0, _totalEnemiesAbleToShoot);
                SIEnemyGridEvents.BroadcastOnShotInvoked(_enemiesAbleToShoot[indexOfSelectedEnemy]);
                yield return WaitUtils.WaitForCachedSeconds(Random.Range(
                    _gridBehaviourSettings.minShootingInterval, _gridBehaviourSettings.maxShootingInterval));
            }
        }
    }
}