using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerShootBehaviour : SIShootBehaviour
    {
        protected override void AssignEvents()
        {
            SIEventsHandler.OnShootInputReceived += HandleOnShootInputReceived;
        }

        protected override void RemoveEvents()
        {
            SIEventsHandler.OnShootInputReceived -= HandleOnShootInputReceived;
        }

        void HandleOnShootInputReceived()
        {
            TryToShootProjectile();
        }

        protected override void TryToShootProjectile()
        {
            weaponReloader.TryToShootAndReload();
        }

        public void Debug_Shot()
        {
            TryToShootProjectile();
        }
    }
}

