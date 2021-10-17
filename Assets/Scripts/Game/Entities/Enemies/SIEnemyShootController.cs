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
            SIGameplayEvents.OnWaveEnd += HandleOnWaveEnd;
            SIGameplayEvents.OnEnemyProjectilesCountChanged += HandleOnEnemyProjectilesCountChanged;
        }

        protected override void UnsubscribeEvents()
        {
            SIGameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
            SIGameplayEvents.OnEnemyProjectilesCountChanged -= HandleOnEnemyProjectilesCountChanged;
        }

        void HandleOnWaveEnd()
        {
            SIEnemyGridEvents.BroadcastOnReadyToShoot(this);
        }

        void HandleOnEnemyProjectilesCountChanged(int availableProjectiles) {
            _availableProjectilesCount = availableProjectiles;
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