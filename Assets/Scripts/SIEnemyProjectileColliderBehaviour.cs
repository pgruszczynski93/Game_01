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
                _onCollisionActions[_objectTags[i]] += _colliderParentBehaviour.OnCollisionResetProjectile;
                _onCollisionActions[_objectTags[i]] += OnCollisionMessage;
            }
        }

        protected override void OnDisable()
        {
            for (int i = 0; i < _objectTags.Length; i++)
            {
                _onCollisionActions[_objectTags[i]] -= EnableDestroyVFX;
                _onCollisionActions[_objectTags[i]] -= _colliderParentBehaviour.OnCollisionResetProjectile;
                _onCollisionActions[_objectTags[i]] -= OnCollisionMessage;
            }
        }

        private void OnCollisionMessage(MonoBehaviour collisionBehaviour = null)
        {
            SIHelpers.SISimpleLogger(this, gameObject.name + " - collision detected ", SimpleLoggerTypes.Log);
        }

        private void EnableDestroyVFX(MonoBehaviour collisionBehaviour = null)
        {
            // p[oprawić efekt pocisku tak by zostawał w miejscu po uderzeniu!! - zrobic gameobject ktory sie odczepia w momencie uderzenia i ma efekt - potem powraca do pozycji poczatkowej
            if (_destroyVFX != null)
            {
                _destroyVFX.OnEnableAndDetachVFXCallback(true);
            }
        }
    }
}