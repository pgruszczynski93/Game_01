using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIProjectileColliderBehaviour : SIMainColliderBehaviour<SIProjectileBehaviour>
    {
        protected override void OnEnable()
        {
            onCollisionCallback += _colliderParentBehaviour.ResetProjectile;
        }

        protected override void OnDisable()
        {
            onCollisionCallback -= _colliderParentBehaviour.ResetProjectile;
        }
    }
}
