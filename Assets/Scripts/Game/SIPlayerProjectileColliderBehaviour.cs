
using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerProjectileColliderBehaviour : SIMainColliderBehaviour<SIProjectileBehaviour>
    {
        protected override void OnEnable()
        {

            for (int i = 0; i < _objectTags.Length; i++)
            {
                _onCollisionActions[_objectTags[i]] += _colliderParentBehaviour.OnPlayerProjectileHitsEnemy;
                _onCollisionActions[_objectTags[i]] += OnCollisionMessage;
            }
        }

        protected override void OnDisable()
        {
            for (int i = 0; i < _objectTags.Length; i++)
            {
                _onCollisionActions[_objectTags[i]] -= _colliderParentBehaviour.OnPlayerProjectileHitsEnemy;
                _onCollisionActions[_objectTags[i]] -= OnCollisionMessage;
            }
        }

        private void OnCollisionMessage(MonoBehaviour collisionBehaviour = null)
        {
            SIHelpers.SISimpleLogger(this, gameObject.name + " - collision detected ", SimpleLoggerTypes.Log);
        }
    }
}
