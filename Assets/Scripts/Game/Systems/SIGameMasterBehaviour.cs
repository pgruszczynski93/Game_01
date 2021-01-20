using UnityEngine;

namespace SpaceInvaders
{
    public class SIGameMasterBehaviour : SIGenericSingleton<SIGameMasterBehaviour> {
        [SerializeField] SIGameStatistics _gameStatistics;
        [SerializeField] Camera _mainCamera;
        [SerializeField] SIPlayerBehaviour _player;
        [SerializeField] SIScreenAreaCalculator _screeenAreaCalculator;
        [SerializeField] SIMaterialCache _materialCache;

        public Camera MainCamera => _mainCamera;
        public SIPlayerBehaviour Player => _player;
        public SIScreenAreaCalculator ScreenAreaCalculator => _screeenAreaCalculator;
        public SIMaterialCache MaterialCache => _materialCache;

        void Start() => Initialise();

        void Initialise()
        {
            _gameStatistics = new SIGameStatistics();
        }
        
        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents()
        {
            // add 
            SIGameplayEvents.OnEnemyDeath += HandleOnEnemyDeath;
            SIGameplayEvents.OnWaveEnd += HandleOnWaveEnd;
            SIBonusesEvents.OnBonusCollected += HandleOnBonusCollected;
            SIGameplayEvents.OnEnemyWeaponHit += HandleOnEnemyWeaponHit;
            //update score here
        }

        void UnsubscribeEvents()
        {
            SIGameplayEvents.OnEnemyDeath -= HandleOnEnemyDeath;
            SIGameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
            SIBonusesEvents.OnBonusCollected -= HandleOnBonusCollected;
            SIGameplayEvents.OnEnemyWeaponHit -= HandleOnEnemyWeaponHit;
        }

        void HandleOnEnemyDeath(SIEnemyBehaviour enemy)
        {
            _gameStatistics.UpdatePlayerKillsCounter();
        }
        void HandleOnWaveEnd()
        {
            _gameStatistics.UpdateCurrentWaveCounter();
        }
        
        void HandleOnBonusCollected(BonusSettings bonus)
        {
            _gameStatistics.UpdatePlayerCollectedBonusesCounter();
        }
        
        void HandleOnEnemyWeaponHit()
        {
            _gameStatistics.UpdateEnemyBulletsDestroyedCounter();
        }
    }
}