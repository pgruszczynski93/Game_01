using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerShootBehaviour : SIShootBehaviour
    {
        protected override void AssignEvents()
        {
            SIEventsHandler.OnShootInputReceived += TryToShootProjectile;
        }

        protected override void RemoveEvents()
        {
            SIEventsHandler.OnShootInputReceived -= TryToShootProjectile;
        }

        protected override void TryToShootProjectile()
        {
            if (!SIEnemiesGridsMaster.Instance.IsEnemyInGridMovementAllowed)
                return;
            
            weaponReloader.TryToShootAndReload();
        }

        public void Debug_Shot()
        {
            weaponReloader.TryToShootAndReload();
        }
    }
}

