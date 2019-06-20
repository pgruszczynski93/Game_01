using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyShootBehaviour : SIShootBehaviour<SIProjectileBehaviour>, IShootable
    {
        protected override void InvokeShoot()
        {
            _projectileController.gameObject.SetActive(true);
            _projectileController.MoveProjectile();
        }

        public void Shoot()
        {
            InvokeShoot();
        }
    }

}
