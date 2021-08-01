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
            
        }

        public void Debug_Shot()
        {
        }
    }
}

