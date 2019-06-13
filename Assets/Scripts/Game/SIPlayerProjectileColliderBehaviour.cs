
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
            }
        }

        protected override void OnDisable()
        {
            for (int i = 0; i < _objectTags.Length; i++)
            {
                _onCollisionActions[_objectTags[i]] -= _colliderParentBehaviour.OnPlayerProjectileHitsEnemy;
            }
        }
    }
}
