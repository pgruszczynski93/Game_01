using UnityEngine;

namespace SpaceInvaders
{
    public class SIGameMasterBehaviour : SIGenericSingleton<SIGameMasterBehaviour> {
        [SerializeField] SIGameStatistics _gameStatistics;
        [SerializeField] Camera _mainCamera;
        [SerializeField] SIPlayerBehaviour _player;
        [SerializeField] SIScreenAreaCalculator _screeenAreaCalculator;

        public Camera MainCamera
        {
            get
            {
                if (_mainCamera != null)
                    return _mainCamera;

                Debug.LogError("No camera assigned!");
                return null;

            }
        }

        public SIPlayerBehaviour Player
        {
            get
            {
                if (_player != null)
                    return _player;

                Debug.LogError("No player assigned to SIGameMasterBehaviour");
                return null;
            }
        }

        public SIScreenAreaCalculator ScreenAreaCalculator => _screeenAreaCalculator;

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