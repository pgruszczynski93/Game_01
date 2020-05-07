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

        void Start()
        {
            SubscribeForInitialShooting();
        }

        protected override void AssignEvents()
        {
//            SIEventsHandler.OnShootInputReceived += TryToShootProjectile;
        }

        protected override void RemoveEvents()
        {
//            SIEventsHandler.OnShootInputReceived -= TryToShootProjectile;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                TryToShootProjectile();
            }
        }

        protected override void TryToShootProjectile()
        {
            if (!_canShoot)
                return;
            weaponReloader.TryToShootAndReload();
        }

        void SubscribeForInitialShooting()
        {
            SIEnemyGridEvents.BroadcastOnSubscribeToShooting(this);
        }

        public void TryToSelectNextShootingBehaviour()
        {
            _canShoot = false;

            //todo: fix thsis
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