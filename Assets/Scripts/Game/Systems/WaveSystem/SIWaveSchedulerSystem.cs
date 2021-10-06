using System.Collections;
using Configs;
using UnityEngine;

namespace SpaceInvaders {
    public class SIWaveSchedulerSystem : MonoBehaviour {

        //ToDo: Add More Waves in future.
        [SerializeField] WaveSettings[] _waveSettings;
        
        int _waveCounter;
        int _maxEnemiesInWave;
        int _livingEnemies;
        
        WaveSettings _currentWaveSettings;
        Coroutine _waveRestartRoutine;
        
        void Start() => Initialise();

        void OnDestroy() {
            if(_waveRestartRoutine != null)
                StopCoroutine(StartNewWaveRoutine());
        }

        void Initialise() {
            if (_waveSettings == null || _waveSettings.Length == 0) {
                Debug.LogError("Assign wave settings!");
                return;
            }

            _currentWaveSettings = _waveSettings[0];
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

            _waveRestartRoutine = StartCoroutine(StartNewWaveRoutine());
        }
        
        void HandleOnEnemyDeath(SIEnemyBehaviour enemy) {
            --_livingEnemies;
            TryToFinalizeWave();
        }
        
        void TryToFinalizeWave()
        {
            if (_livingEnemies > 0)
                return;

            _waveRestartRoutine = StartCoroutine(StartNewWaveRoutine());
        }
        
        IEnumerator StartNewWaveRoutine() {
            //Note: End wave is always before start wave, to ensure that every gameobject which uses this event handled necessary operations.
            yield return StartCoroutine(WaitUtils.WaitAndInvoke(_currentWaveSettings.endWaveCooldown, SIGameplayEvents.BroadcastOnWaveEnd));
            yield return StartCoroutine(WaitUtils.WaitAndInvoke(_currentWaveSettings.newWaveCooldown, SIGameplayEvents.BroadcastOnWaveStart));
        }
    }
}