using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerShootBehaviour : SIShootBehaviour<SIPlayerProjectilesController>
    {
        protected override void OnEnable()
        {
            SIEventsHandler.OnPlayerShoot += Shoot;
            SIEventsHandler.OnObjectMovement += SIEventsHandler.OnPlayerShoot;
        }

        protected override void OnDisable()
        {
            SIEventsHandler.OnPlayerShoot -= Shoot;
            SIEventsHandler.OnObjectMovement -= SIEventsHandler.OnPlayerShoot;
        }

        protected override void Shoot()
        {
            if (Input.GetKeyDown(KeyCode.Space))// && SIEnemiesGridsMaster.Instance.IsEnemyInGridMovementAllowed */)
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

