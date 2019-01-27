using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerShootBehaviour : SIShootBehaviour
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _projectileController.Shoot();
            }
        }
    }
}

