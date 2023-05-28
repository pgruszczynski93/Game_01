using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PG.Game.Configs;
using PG.Game.Entities.Enemies;
using PG.Game.EventSystem;
using PG.Game.Systems;
using UnityEngine;
using WaveType = PG.Game.Systems.WaveSystem.WaveType;

namespace PG.Game.Entities {
    public class EnemiesGridShootController : MonoBehaviour {
        [SerializeField] bool _isGridShootingLockedByDev;

        [SerializeField] GridShootingSetup _gridBehaviourSetup;
        [SerializeField] List<EnemyShootController> _enemiesAbleToShoot;

        bool _isGridShootingEnabled;
        int _totalEnemiesAbleToShoot;
        GridShootingSettings _gridBehaviourSettings;
        CancellationTokenSource _shootingCancellation;

        void Start() {
            Initialise();
        }

        void Initialise() {
            _gridBehaviourSettings = _gridBehaviourSetup.shootingSettings;
            _enemiesAbleToShoot = new List<EnemyShootController>();
        }

        void OnEnable() {
            SubscribeEvents();
        }

        void OnDisable() {
            UnsubscribeEvents();
        }

        void SubscribeEvents() {
            GameplayEvents.OnWaveStart += HandleOnWaveStart;
            GameplayEvents.OnWaveEnd += HandleOnWaveEnd;
            GameplayEvents.OnWaveCoolDown += HandleOnWaveCoolDown;
            EnemyGridEvents.OnReadyToShoot += HandleOnReadyToShoot;
            GameplayEvents.OnEnemyDeath += HandleOnEnemyDeath;
        }

        void UnsubscribeEvents() {
            GameplayEvents.OnWaveStart -= HandleOnWaveStart;
            GameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
            GameplayEvents.OnWaveCoolDown += HandleOnWaveCoolDown;
            EnemyGridEvents.OnReadyToShoot -= HandleOnReadyToShoot;
            GameplayEvents.OnEnemyDeath -= HandleOnEnemyDeath;
        }

        void HandleOnReadyToShoot(EnemyShootController enemyShootController) {
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
            List<EnemyShootController> initialShootBehaviours = new();
            EnemyShootController currentController;
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

        void HandleOnEnemyDeath(EnemyBehaviour deadEnemy) {
            TryToUpdateShootingEnemies(deadEnemy.EnemyShootController);
        }

        void TryToUpdateShootingEnemies(EnemyShootController deadEnemyShootController) {
            if (!EnemyAbleToShootKilled(deadEnemyShootController))
                return;

            _enemiesAbleToShoot.Remove(deadEnemyShootController);
            SetEnemiesAbleToShootCount();
        }

        bool EnemyAbleToShootKilled(EnemyShootController deadEnemyShootController) {
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

            while (_totalEnemiesAbleToShoot > 0)
                await WaitUtils.StartWaitSecFinishTask(
                    () => {
                        indexOfSelectedEnemy = Random.Range(0, _totalEnemiesAbleToShoot);
                        EnemyGridEvents.BroadcastOnShotInvoked(_enemiesAbleToShoot[indexOfSelectedEnemy]);
                    },
                    null,
                    Random.Range(_gridBehaviourSettings.minShootingInterval, _gridBehaviourSettings.maxShootingInterval),
                    _shootingCancellation.Token);
        }

        void RefreshCancellation() {
            _shootingCancellation?.Cancel();
            _shootingCancellation?.Dispose();
            _shootingCancellation = new CancellationTokenSource();
        }
    }
}