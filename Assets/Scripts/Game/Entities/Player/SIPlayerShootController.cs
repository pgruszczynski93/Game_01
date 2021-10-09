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
            SIGameplayEvents.OnPlayerWeaponTierUpdate += HandleOnPlayerWeaponTierUpdate;
            SIEnemyGridEvents.OnGridOnGridVisibilityChanged += HandleOnGridOnGridVisibilityChanged;
        }

        protected override void UnsubscribeEvents() {
            SIEventsHandler.OnUpdate += HandleOnUpdate;
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
            SIGameplayEvents.OnPlayerWeaponTierUpdate -= HandleOnPlayerWeaponTierUpdate;
            SIEnemyGridEvents.OnGridOnGridVisibilityChanged -= HandleOnGridOnGridVisibilityChanged;
        }
        
        void HandleOnBonusEnabled(BonusSettings bonusSettings) {
            switch (bonusSettings.bonusType) {
                case BonusType.LaserBeam:
                    EnableShooting(false);
                    break;
                case BonusType.RapidFire:
                    EnableRapidFire();
                    break;
            }
        }
        
        void HandleOnBonusDisabled(BonusSettings bonusSettings) {
            switch (bonusSettings.bonusType) {
                case BonusType.LaserBeam:
                    EnableShooting(true);
                    break;
                case BonusType.RapidFire:
                    DisableRapidFire();
                    break;
            }
        }

        void HandleOnPlayerWeaponTierUpdate(WeaponTier weaponTier) {
            _projectilesTier = (int) weaponTier;
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
