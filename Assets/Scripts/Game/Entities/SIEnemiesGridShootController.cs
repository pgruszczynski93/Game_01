using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SpaceInvaders {
    public class SIEnemiesGridShootController : MonoBehaviour {
        [SerializeField] bool _isGridShootingLockedByDev;

        [SerializeField] GridShootingSetup _gridBehaviourSetup;
        [SerializeField] List<SIEnemyShootController> _enemiesAbleToShoot;

        bool _isGridShootingEnabled;
        int _totalEnemiesAbleToShoot;
        GridShootingSettings _gridBehaviourSettings;
        CancellationTokenSource _shootingCancellation;

        void Start() {
            Initialise();
        }

        void Initialise() {
            _gridBehaviourSettings = _gridBehaviourSetup.shootingSettings;
            _enemiesAbleToShoot = new List<SIEnemyShootController>();
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

        void HandleOnWaveStart(WaveType waveType) {
            if (waveType != WaveType.Grid)
                return;
            
            EnableGridShootingPossibility(true);
            TryToRunGridShootingTask();
        }

        void HandleOnWaveEnd(WaveType waveType) {
            // if (waveType != WaveType.Grid)
                // return;

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

        void TryToRunGridShootingTask() {
            if (ShouldStopGridShooting() || _isGridShootingLockedByDev)
                return;
            
            RefreshCancellation();
            GridShootingTask().Forget();
        }


        async UniTaskVoid GridShootingTask() {
            int indexOfSelectedEnemy;

            while (_totalEnemiesAbleToShoot > 0) {
                await WaitUtils.StartWaitSecFinishTask(
                    () => {
                        indexOfSelectedEnemy = Random.Range(0, _totalEnemiesAbleToShoot);
                        SIEnemyGridEvents.BroadcastOnShotInvoked(_enemiesAbleToShoot[indexOfSelectedEnemy]);
                    }, 
                    null,
                    Random.Range(_gridBehaviourSettings.minShootingInterval, _gridBehaviourSettings.maxShootingInterval),
                    _shootingCancellation.Token);
            }
        }
        
        void RefreshCancellation() {
            _shootingCancellation?.Cancel();
            _shootingCancellation?.Dispose();
            _shootingCancellation = new CancellationTokenSource();
        }
    }
}