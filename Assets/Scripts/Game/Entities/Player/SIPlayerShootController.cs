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
            SIEnemyGridEvents.OnGridShootingReset += HandleOnGridShootingReset;
            SIGameplayEvents.OnPlayerWeaponTierUpdate += HandleOnPlayerWeaponTierUpdate;
            SIGameplayEvents.OnWaveStart += HandleOnWaveStart;
            SIGameplayEvents.OnWaveEnd += HandleOnWaveEnd;
        }

        protected override void UnsubscribeEvents() {
            SIEventsHandler.OnUpdate += HandleOnUpdate;
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
            SIGameplayEvents.OnPlayerWeaponTierUpdate -= HandleOnPlayerWeaponTierUpdate;
            SIGameplayEvents.OnWaveStart -= HandleOnWaveStart;
            SIGameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
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
        void HandleOnGridShootingReset() {
            _isShootingEnabled = true;
        }
        
        void HandleOnWaveStart() {
            SetAutoShooting(true);
        }
        void HandleOnWaveEnd() {
            SetAutoShooting(false);
        }

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
