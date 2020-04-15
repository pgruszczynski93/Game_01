using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyShootBehaviour :  SIShootBehaviour
    {
        bool canShoot;
        [SerializeField] int _initialShootingMinIndex;
        [SerializeField] int _shootingEnemyIndex;

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
            if (Input.GetKeyDown(KeyCode.K) && canShoot)
            {
                TryToShootProjectile();
            }
        }
        protected override void TryToShootProjectile()
        {
            weaponReloader.TryToShootAndReload();
        }

        //fixnąć indesksy 
        void SubscribeForInitialShooting()
        {
            if (_shootingEnemyIndex < _initialShootingMinIndex)
                return;

            canShoot = true;
            SIEnemyGridEvents.BroadcastOnSubscribeToShooting(this);
        }
    }

}
