using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyShootBehaviour : SIShootBehaviour
    {
        [SerializeField] SIShootBehaviourSetup shootBehaviourSetup;

        public SIShootBehaviourSetup ShootBehaviourSetup
        {
            get => shootBehaviourSetup;
            set => shootBehaviourSetup = value;
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
            weaponReloader.TryToShootAndReload();
        }

        void SubscribeForInitialShooting()
        {
            SIEnemyGridEvents.BroadcastOnSubscribeToShooting(this);
        }
    }
}