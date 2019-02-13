
using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerProjectileColliderBehaviour : SIMainColliderBehaviour<SIProjectileBehaviour>
    {
        protected override void OnEnable()
        {
            _onCollisionActions["Enemy"] += _colliderParentBehaviour.OnCollisionResetProjectile;
            _onCollisionActions["Enemy"] += OnCollisionMessage;
        }

        protected override void OnDisable()
        {
            _onCollisionActions["Enemy"] -= _colliderParentBehaviour.OnCollisionResetProjectile;
            _onCollisionActions["Enemy"] -= OnCollisionMessage;
        }

        private void OnCollisionMessage(MonoBehaviour collisionBehaviour = null)
        {
            SIHelpers.SISimpleLogger(this, gameObject.name + " - collision detected ", SimpleLoggerTypes.Log);
        }
    }
}
