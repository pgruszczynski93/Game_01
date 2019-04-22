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
                _onCollisionActions[_objectTags[i]] += OnCollisionMessage;
            }
        }

        protected override void OnDisable()
        {
            for (int i = 0; i < _objectTags.Length; i++)
            {
                _onCollisionActions[_objectTags[i]] -= EnableDestroyVFX;
                _onCollisionActions[_objectTags[i]] -= _colliderParentBehaviour.OnEnemyDeathResetProjectile;
                _onCollisionActions[_objectTags[i]] -= OnCollisionMessage;
            }
        }

        private void OnCollisionMessage(MonoBehaviour collisionBehaviour = null)
        {
            SIHelpers.SISimpleLogger(this, gameObject.name + " - collision detected ", SimpleLoggerTypes.Log);
        }

        private void EnableDestroyVFX(MonoBehaviour collisionBehaviour = null)
        {
            if (_destroyVFX != null)
            {
                _destroyVFX.OnEnableAndDetachVFXCallback(true);
            }
        }
    }
}