using PG.Game.Configs;
using PG.Game.Entities.Enemies;
using PG.Game.Entities.Player;
using PG.Game.EventSystem;
using PG.Game.Helpers;
using UnityEngine;
using WaveType = PG.Game.Systems.WaveSystem.WaveType;

namespace PG.Game.Systems {
    public class GameMasterBehaviour : GenericSingleton<GameMasterBehaviour> {
        [SerializeField] GameStatistics _gameStatistics;
        [SerializeField] Camera _mainCamera;
        [SerializeField] PlayerBehaviour _player;
        [SerializeField] ScreenAreaCalculator _screeenAreaCalculator;

        public Camera MainCamera => _mainCamera;
        public PlayerBehaviour Player => _player;
        public ScreenAreaCalculator ScreenAreaCalculator => _screeenAreaCalculator;

        void Start() => Initialise();

        void Initialise() {
            _gameStatistics = new GameStatistics();
        }

        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();

        void SubscribeEvents() {
            // add 
            GameplayEvents.OnEnemyDeath += HandleOnEnemyDeath;
            GameplayEvents.OnWaveEnd += HandleOnWaveEnd;
            BonusesEvents.OnBonusCollected += HandleOnBonusCollected;
            GameplayEvents.OnEnemyWeaponHit += HandleOnEnemyWeaponHit;
            //update score here
        }

        void UnsubscribeEvents() {
            GameplayEvents.OnEnemyDeath -= HandleOnEnemyDeath;
            GameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
            BonusesEvents.OnBonusCollected -= HandleOnBonusCollected;
            GameplayEvents.OnEnemyWeaponHit -= HandleOnEnemyWeaponHit;
        }

        void HandleOnEnemyDeath(EnemyBehaviour enemy) {
            _gameStatistics.UpdatePlayerKillsCounter();
        }

        void HandleOnWaveEnd(WaveType waveType) {
            //Todo: Consider usage of waveType
            _gameStatistics.UpdateCurrentWaveCounter();
        }

        void HandleOnBonusCollected(BonusSettings bonus) {
            _gameStatistics.UpdatePlayerCollectedBonusesCounter();
        }

        void HandleOnEnemyWeaponHit() {
            _gameStatistics.UpdateEnemyBulletsDestroyedCounter();
        }
    }
}