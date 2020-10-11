namespace SpaceInvaders
{
    public class SIPlayerShootBehaviour : SIShootBehaviour
    {
        protected override void AssignEvents()
        {
            SIGameplayEvents.OnPlayerShoot += HandleOnPlayerShoot;
        }

        protected override void RemoveEvents()
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

