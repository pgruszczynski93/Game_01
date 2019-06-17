using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyProjectileColliderBehaviour : SIMainColliderBehaviour<SIProjectileBehaviour>
    {
        [SerializeField] private SIVFXManager _destroyVFX;

        protected override void OnEnable()
        {
            for (int i = 0; i < _objectTags.Length; i++)
            {
                _onCollisionActions[_objectTags[i]] += EnableDestroyVFX;
                _onCollisionActions[_objectTags[i]] += _colliderParentBehaviour.OnEnemyDeathResetProjectile;
            }
        }

        protected override void OnDisable()
        {
            for (int i = 0; i < _objectTags.Length; i++)
            {
                _onCollisionActions[_objectTags[i]] -= EnableDestroyVFX;
                _onCollisionActions[_objectTags[i]] -= _colliderParentBehaviour.OnEnemyDeathResetProjectile;
            }
        }

        private void EnableDestroyVFX(MonoBehaviour collisionBehaviour = null)
        {
            if (_destroyVFX == null)
                return;
            
            Debug.Log("UDERZAM");

            _destroyVFX.HandleOnEnableAndDetachVFX(true);
        }
    }
}