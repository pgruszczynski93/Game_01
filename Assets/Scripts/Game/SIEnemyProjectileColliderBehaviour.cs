using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyProjectileColliderBehaviour : SIMainColliderBehaviour<SIWeaponEntity>
    {
        [SerializeField] private SIVFXManager _destroyVFX;

        protected override void OnEnable()
        {
            for (int i = 0; i < _objectTags.Length; i++)
            {
                _onCollisionActions[_objectTags[i]] += EnableDestroyVfx;
                _onCollisionActions[_objectTags[i]] += _colliderParentBehaviour.HandleProjectileHit;
            }
        }

        protected override void OnDisable()
        {
            for (int i = 0; i < _objectTags.Length; i++)
            {
                _onCollisionActions [_objectTags[i]] -= EnableDestroyVfx;
                _onCollisionActions[_objectTags[i]] -= _colliderParentBehaviour.HandleProjectileHit;
            }
        }

        private void EnableDestroyVfx(MonoBehaviour collisionBehaviour = null)
        {
            if (_destroyVFX == null)
                return;
            
            _destroyVFX.TryToEnableAndDetachVFX(true);
        }
    }
}