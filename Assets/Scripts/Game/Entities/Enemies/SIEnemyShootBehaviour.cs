using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyShootBehaviour : SIShootBehaviour
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

        protected override void AssignEvents()
        {
            SIEnemyGridEvents.OnGridReset += HandleOnGridReset;
            SIEnemyGridEvents.OnShootOrderRequested += HandleOnShootOrderRequested;
//            SIEventsHandler.OnShootInputReceived += TryToShootProjectile;
        }

        protected override void RemoveEvents()
        {
            SIEnemyGridEvents.OnGridReset -= HandleOnGridReset;
            SIEnemyGridEvents.OnShootOrderRequested -= HandleOnShootOrderRequested;
//            SIEventsHandler.OnShootInputReceived -= TryToShootProjectile;
        }

        void HandleOnGridReset()
        {
            SubscribeToShooting();
        }

//        void Update()
//        {
//            if (Input.GetKeyDown(KeyCode.K))
//            {
//                TryToShootProjectile();
//            }
//        }

        void HandleOnShootOrderRequested(SIEnemyShootBehaviour requestedBehaviour)
        {
            if (this != requestedBehaviour)
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
            SIEnemyGridEvents.BroadcastOnSubscribeToShooting(this);
        }

        public void TryToSelectNextShootingBehaviour()
        {
            _canShoot = false;

            SIEnemyBehaviour closerRowCandidate = shootBehaviourSetup.neighbours[Neighbour.Back];
            if (closerRowCandidate == null)
                return;

            SIEnemyBehaviour furtherRowCandidate =
                closerRowCandidate.ShootBehaviour.ShootBehaviourSetup.neighbours[Neighbour.Back];

            SIEnemyShootBehaviour nextSelected = closerRowCandidate.IsEnemyAlive()
                ? closerRowCandidate.ShootBehaviour
                : furtherRowCandidate != null && furtherRowCandidate.IsEnemyAlive()
                    ? furtherRowCandidate.ShootBehaviour
                    : null;

            if (nextSelected == null)
                return;

            nextSelected.CanShoot = true;
            SIEnemyGridEvents.BroadcastOnSubscribeToShooting(nextSelected);
        }
    }
}