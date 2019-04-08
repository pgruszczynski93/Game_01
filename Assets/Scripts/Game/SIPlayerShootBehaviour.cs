using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerShootBehaviour : SIShootBehaviour<SIPlayerProjectilesController>
    {
        protected override void OnEnable()
        {
            SIEventsHandler.OnObjectsMovement += InvokeShoot;
        }

        protected override void OnDisable()
        {
            SIEventsHandler.OnObjectsMovement -= InvokeShoot;
        }

        protected override void InvokeShoot()
        {
            if (Input.GetKeyDown(KeyCode.Space) && SIEnemiesGridsMaster.Instance.IsEnemyInGridMovementAllowed)
            {
                _projectileController.Shoot();
            }
        }

        public void Debug_Shot()
        {
            _projectileController.Shoot();
        }
    }
}

