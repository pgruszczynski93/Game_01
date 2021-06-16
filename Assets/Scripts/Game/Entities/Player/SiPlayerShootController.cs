namespace SpaceInvaders
{
    public class SiPlayerShootController : SIShootController
    {
        protected override void SubscribeEvents()
        {
            SIGameplayEvents.OnPlayerShoot += HandleOnPlayerShoot;
        }

        protected override void UnsubscribeEvents()
        {
            SIGameplayEvents.OnPlayerShoot -= HandleOnPlayerShoot;
        }

        void HandleOnPlayerShoot()
        {
            TryToShootProjectile();
        }

        protected override void TryToShootProjectile()
        {
            weaponReloader.TryToShootAndReload();
        }

        public void Debug_Shot()
        {
            TryToShootProjectile();
        }
    }
}

