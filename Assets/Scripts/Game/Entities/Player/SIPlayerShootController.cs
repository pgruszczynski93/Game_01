namespace SpaceInvaders
{
    public class SIPlayerShootController : SIShootController {

        protected override void SubscribeEvents() {
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
            SIGameplayEvents.OnPlayerWeaponTierUpdate += HandleOnPlayerWeaponTierUpdate;
        }

        protected override void UnsubscribeEvents() {
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
            SIGameplayEvents.OnPlayerWeaponTierUpdate -= HandleOnPlayerWeaponTierUpdate;
        }

        void HandleOnBonusDisabled(BonusSettings bonusSettings) {
            SetShootingStatus(bonusSettings.bonusType != BonusType.LaserBeam);
        }

        void HandleOnBonusEnabled(BonusSettings bonusSettings) {
            SetShootingStatus(bonusSettings.bonusType == BonusType.LaserBeam);
        }

        void HandleOnPlayerWeaponTierUpdate(WeaponTier weaponTier) {
            _projectilesTier = (int) weaponTier;
        }
    }
}
