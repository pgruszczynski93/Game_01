using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyShootBehaviour :  SIShootBehaviour
    {
        public int shootingEnemyIndex;

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
    }

}
