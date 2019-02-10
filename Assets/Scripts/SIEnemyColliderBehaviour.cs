using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyColliderBehaviour : SIMainColliderBehaviour<SIEnemyBehaviour>
    {

        protected override void OnEnable()
        {
            _onCollisionActions["Projectile"] += _colliderParentBehaviour.Death;
            _onCollisionActions["Projectile"] += InvokeEnemyDeathCallback;
            _onCollisionActions["Projectile"] += OnCollisionMessage;
        }

        protected override void OnDisable()
        {
            _onCollisionActions["Projectile"] -= _colliderParentBehaviour.Death;
            _onCollisionActions["Projectile"] -= InvokeEnemyDeathCallback;
            _onCollisionActions["Projectile"] -= OnCollisionMessage;
        }

        private void InvokeEnemyDeathCallback(MonoBehaviour collisionBehaviour = null)
        {
            SIEventsHandler.OnEnemyDeath?.Invoke();
        }

        private void OnCollisionMessage(MonoBehaviour collisionBehaviour = null)
        {
            SIHelpers.SISimpleLogger(this, gameObject.name + " - collision detected ", SimpleLoggerTypes.Log);
        }
    }
}
