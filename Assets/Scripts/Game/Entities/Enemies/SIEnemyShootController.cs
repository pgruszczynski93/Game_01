using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyShootController : SIShootController
    {
        [SerializeField] SIShootBehaviourSetup shootBehaviourSetup;

        bool _canShoot;
        
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
            SIGameplayEvents.OnEnemyWeaponTierUpdate += HandleOnEnemyWeaponTierUpdate;
        }

        protected override void UnsubscribeEvents()
        {
            SIEnemyGridEvents.OnGridObjectsReloaded -= HandleOnGridObjectsReloaded;
            SIGameplayEvents.OnEnemyWeaponTierUpdate -= HandleOnEnemyWeaponTierUpdate;
        }

        void HandleOnGridObjectsReloaded()
        {
            SubscribeToShooting();
        }

        void HandleOnEnemyWeaponTierUpdate(WeaponTier weaponTier) {
            _projectilesTier = (int) weaponTier;
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