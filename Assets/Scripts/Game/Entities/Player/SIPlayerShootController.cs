using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerShootController : SIShootController {

        [SerializeField] bool _canAutoShoot;
        [SerializeField] float _basicShootInterval;
        [SerializeField] float _rapidShootInterval;
        
        float _nextShootCountdown;
        float _currentShootInterval;

        protected override void Initialise() {
            base.Initialise();
            _currentShootInterval = _basicShootInterval;
            _nextShootCountdown = _currentShootInterval;
        }

        protected override void SubscribeEvents() {
            SIEventsHandler.OnUpdate += HandleOnUpdate;
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
            SIGameplayEvents.OnPlayerProjectilesCountChanged += HandleOnPlayerProjectilesCountChanged;
            SIEnemyGridEvents.OnGridOnGridVisibilityChanged += HandleOnGridOnGridVisibilityChanged;
        }

        protected override void UnsubscribeEvents() {
            SIEventsHandler.OnUpdate += HandleOnUpdate;
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
            SIGameplayEvents.OnPlayerProjectilesCountChanged -= HandleOnPlayerProjectilesCountChanged;
            SIEnemyGridEvents.OnGridOnGridVisibilityChanged -= HandleOnGridOnGridVisibilityChanged;
        }
        
        void HandleOnBonusEnabled(BonusSettings bonusSettings) {
            switch (bonusSettings.bonusType) {
                case BonusType.LaserBeam:
                    EnableShooting(false);
                    break;
                case BonusType.EnergyBoost:
                    EnableRapidFire();
                    break;
                case BonusType.Projectile:
                    UpdateAvailableProjectilesCount();
                    break;
            }
        }
        
        void HandleOnBonusDisabled(BonusSettings bonusSettings) {
            switch (bonusSettings.bonusType) {
                case BonusType.LaserBeam:
                    EnableShooting(true);
                    break;
                case BonusType.EnergyBoost:
                    DisableRapidFire();
                    break;
                case BonusType.Projectile:
                    //Intentionally I don't want to disable this bonus.
                    break;
            }
        }

        void HandleOnPlayerProjectilesCountChanged(int projectilesCount) {
            _availableProjectilesCount = projectilesCount;
        }
        
        void HandleOnUpdate() {
            ExecuteAutoShooting();
        }
        
        void HandleOnGridOnGridVisibilityChanged(bool isGridVisible) {
            EnableShooting(isGridVisible);
            SetAutoShooting(isGridVisible);
        }

        //Both types of shooting are enabled / disabled intentionally, in case I'd like to disable autoshooting option
        //
        // void HandleOnWaveStart() {
        //     EnableShooting(true);
        //     SetAutoShooting(true);
        // }
        // void HandleOnWaveEnd() {
        //     EnableShooting(false);
        //     SetAutoShooting(false);
        // }

        void ExecuteAutoShooting() {
            if (!CanAutoShoot())
                return;

            _nextShootCountdown -= Time.deltaTime;
            if (_nextShootCountdown > 0)
                return;
            
            _nextShootCountdown = _currentShootInterval;
            SIGameplayEvents.BroadcastOnPlayerShoot();
        }

        bool CanAutoShoot() {
            return _isShootingEnabled && _canAutoShoot;
        }

        void EnableRapidFire() {
            _currentShootInterval = _rapidShootInterval;
        }

        void DisableRapidFire() {
            _currentShootInterval = _basicShootInterval;
        }

        void SetAutoShooting(bool isEnabled) {
            _canAutoShoot = isEnabled;
        }
    }
}
