using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyShootBehaviour :  SIShootBehaviour
    {
        protected override void TryToShootProjectile()
        {
            _projectilesController.gameObject.SetActive(true);
//            _projectilesController.MoveProjectile();
        }

        public void Shoot()
        {
//            InvokeShoot();
        }
    }

}
