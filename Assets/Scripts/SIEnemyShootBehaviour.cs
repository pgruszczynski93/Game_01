using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyShootBehaviour : SIShootBehaviour
    {
        [SerializeField] private bool _isAbleToShoot;

        protected override void OnEnable()
        {
            SIEventsHandler.OnEnemyShoot += Shoot;
        }

        protected override void OnDisable()
        {
            SIEventsHandler.OnEnemyShoot -= Shoot;
        }

        protected override void Shoot()
        {
            Debug.Log("strzelam " + gameObject.name);
        }
    }

}
