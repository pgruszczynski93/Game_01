namespace SpaceInvaders
{
    public class SiPlayerShootController : SIShootController {
        //wyczyscic!!!
        protected override void SubscribeEvents() {
            SIGameplayEvents.OnPlayerWeaponTierUpdate += HandleOnPlayerWeaponTierUpdate;
        }

        protected override void UnsubscribeEvents() {
            SIGameplayEvents.OnPlayerWeaponTierUpdate -= HandleOnPlayerWeaponTierUpdate;
        }

        void HandleOnPlayerWeaponTierUpdate(WeaponTier weaponTier) {
            _projectilesTier = (int) weaponTier;
        }
    }
}
