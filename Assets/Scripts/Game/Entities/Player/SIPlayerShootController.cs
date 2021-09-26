using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerShootController : SIShootController {

        [SerializeField] bool _canAutoShoot;
        [SerializeField] float _basicShootInterval;
        [SerializeField] float _rapidShootInterval;
        [SerializeField] float _currentShootInterval;
        
        float _nextShootCountdown;

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
        }

        protected override void UnsubscribeEvents() {
            SIEventsHandler.OnUpdate += HandleOnUpdate;
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
            SIGameplayEvents.OnPlayerWeaponTierUpdate -= HandleOnPlayerWeaponTierUpdate;
        }

        void HandleOnBonusDisabled(BonusSettings bonusSettings) {
            switch (bonusSettings.bonusType) {
                case BonusType.LaserBeam:
                    EnableShooting(true);
                    break;
                case BonusType.RapidFire:
                    EnableRapidFire();
                    break;
            }
        }

        void HandleOnBonusEnabled(BonusSettings bonusSettings) {
            switch (bonusSettings.bonusType) {
                case BonusType.LaserBeam:
                    EnableShooting(false);
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

        void ExecuteAutoShooting() {
            if (!CanAutoShoot())
                return;

            _nextShootCountdown -= Time.deltaTime;
            if (_nextShootCountdown > 0)
                return;
            
            _nextShootCountdown = _currentShootInterval;
            // SIGameplayEvents.BroadcastOnPlayerShoot();
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
        
    }
}
