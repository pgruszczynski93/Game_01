using System;
using System.Threading;
using Configs;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class SIWaveSchedulerSystem : MonoBehaviour {

        //ToDo: Add More Waves in future.
        [SerializeField] WaveSettings[] _waveSettings;
        
        int _waveCounter;
        int _livingEnemies;
        
        WaveSettings _currentWaveSettings;
        CancellationTokenSource _waveCancellation;
        
        void Start() => Initialise();

        void Initialise() {
            if (_waveSettings == null || _waveSettings.Length == 0) {
                Debug.LogError("Assign wave settings!");
                return;
            }

            _currentWaveSettings = _waveSettings[0];
            ResetWaveProperties();
        }

        void ResetWaveProperties() {
            _livingEnemies = _currentWaveSettings.enemiesInWave;
        }

        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();

        void SubscribeEvents() {
            SIEventsHandler.OnGameStateChanged += HandleOnGameStateChanged;
            SIGameplayEvents.OnEnemyDeath += HandleOnEnemyDeath;
        }

        void UnsubscribeEvents() {
            SIEventsHandler.OnGameStateChanged -= HandleOnGameStateChanged;
            SIGameplayEvents.OnEnemyDeath -= HandleOnEnemyDeath;
        }
        
        void HandleOnGameStateChanged(GameStates gameState) {
            if (gameState != GameStates.GameStarted)
                return;
            //todo: new wave selector
            StartNewWaveTask(WaveType.Grid).Forget();
        }
        
        void HandleOnEnemyDeath(SIEnemyBehaviour enemy) {
            --_livingEnemies;
            TryToFinalizeWave();
        }
        
        void TryToFinalizeWave()
        {
            if (_livingEnemies > 0)
                return;

            //todo: new wave selector
            StartNewWaveTask(WaveType.Grid).Forget();
        }
        
        async UniTaskVoid StartNewWaveTask(WaveType waveType) {
            //Note: End wave is always before start wave, to ensure that every gameobject which uses this event handled necessary operations.
            try {
                ResetWaveProperties();
                RefreshWaveCancellation();
                await WaitUtils.WaitSecondsAndInvokeTask(_currentWaveSettings.waveEndCoolDown, () => SIGameplayEvents.BroadcastOnWaveEnd(waveType), _waveCancellation.Token);
                await WaitUtils.WaitSecondsAndInvokeTask(_currentWaveSettings.waveCoolDown, SIGameplayEvents.BroadcastOnWaveCoolDown, _waveCancellation.Token);
                await WaitUtils.WaitSecondsAndInvokeTask(_currentWaveSettings.waveStartCooldown, () => SIGameplayEvents.BroadcastOnWaveStart(waveType), _waveCancellation.Token);
            }
            catch (OperationCanceledException) { } 
        }

        void RefreshWaveCancellation() {
            _waveCancellation?.Cancel();
            _waveCancellation?.Dispose();
            _waveCancellation = new CancellationTokenSource();
        }

        [Button]
        void BroadcastNoWave() {
            StartNewWaveTask(WaveType.NoWave).Forget();
        }
        
        [Button]
        void BroadcastGridWave() {
            StartNewWaveTask(WaveType.Grid).Forget();
        }
        
        [Button]
        void BroadcastAsteroidsWave() {
            StartNewWaveTask(WaveType.Asteroids).Forget();
        }
    }
}