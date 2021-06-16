using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyShootController : SIShootController
    {
        bool _canShoot;
        [SerializeField] SIShootBehaviourSetup shootBehaviourSetup;

        public SIShootBehaviourSetup ShootBehaviourSetup
        {
            get => shootBehaviourSetup;
            set => shootBehaviourSetup = value;
        }

        public bool CanShoot
        {
            get => _canShoot;
            set => _canShoot = value;
        }

        protected override void SubscribeEvents()
        {
            SIEnemyGridEvents.OnGridObjectsReloaded += HandleOnGridObjectsReloaded;
            SIEnemyGridEvents.OnShotInvoked += HandleOnShotInvoked;
        }

        protected override void UnsubscribeEvents()
        {
            SIEnemyGridEvents.OnGridObjectsReloaded -= HandleOnGridObjectsReloaded;
            SIEnemyGridEvents.OnShotInvoked -= HandleOnShotInvoked;
        }

        void HandleOnGridObjectsReloaded()
        {
            SubscribeToShooting();
        }

        void HandleOnShotInvoked(SIEnemyShootController requestedController)
        {
            if (this != requestedController)
                return;
            
            TryToShootProjectile();
        }

        protected override void TryToShootProjectile()
        {
            if (!_canShoot)
                return;
            weaponReloader.TryToShootAndReload();
        }

        void SubscribeToShooting()
        {
            SIEnemyGridEvents.BroadcastOnReadyToShoot(this);
        }

        public void TryToSelectNextShootingBehaviour()
        {
            _canShoot = false;

            SIEnemyBehaviour closerRowCandidate = shootBehaviourSetup.neighbours[Neighbour.Back];
            if (closerRowCandidate == null)
                return;

            SIEnemyBehaviour furtherRowCandidate =
                closerRowCandidate.EnemyShootController.ShootBehaviourSetup.neighbours[Neighbour.Back];

            SIEnemyShootController nextSelected = closerRowCandidate.IsEnemyAlive()
                ? closerRowCandidate.EnemyShootController
                : furtherRowCandidate != null && furtherRowCandidate.IsEnemyAlive()
                    ? furtherRowCandidate.EnemyShootController
                    : null;

            if (nextSelected == null)
                return;

            nextSelected.CanShoot = true;
            SIEnemyGridEvents.BroadcastOnReadyToShoot(nextSelected);
        }
    }
}