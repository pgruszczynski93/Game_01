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
            if (bonusSettings.bonusType != BonusType.LaserBeam)
                return;
            EnableShooting(true);
        }

        void HandleOnBonusEnabled(BonusSettings bonusSettings) {
            if (bonusSettings.bonusType != BonusType.LaserBeam)
                return;
            EnableShooting(false);
        }

        void HandleOnPlayerWeaponTierUpdate(WeaponTier weaponTier) {
            _projectilesTier = (int) weaponTier;
        }
    }
}
