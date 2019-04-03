using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyColliderBehaviour : SIMainColliderBehaviour<SIEnemyBehaviour>
    {
        [SerializeField] private SIEnemyShootBehaviour _parentShootBehaviour;

        public SIEnemyShootBehaviour ParentShootBehaviour
        {
            get => _parentShootBehaviour;
        } 
        
        protected override void OnEnable()
        {
            for (int i = 0; i < _objectTags.Length; i++)
            {
                _onCollisionActions[_objectTags[i]] += _colliderParentBehaviour.Death;
                _onCollisionActions[_objectTags[i]] += InvokeEnemyDeathCallback;
                _onCollisionActions[_objectTags[i]] += OnCollisionMessage;
            }
        }

        protected override void OnDisable()
        {
            for (int i = 0; i < _objectTags.Length; i++)
            {
                _onCollisionActions[_objectTags[i]] -= _colliderParentBehaviour.Death;
                _onCollisionActions[_objectTags[i]] -= InvokeEnemyDeathCallback;
                _onCollisionActions[_objectTags[i]] -= OnCollisionMessage;
            }
        }

        private void InvokeEnemyDeathCallback(MonoBehaviour collisionBehaviour = null)
        {
            SIEventsHandler.BroadcastOnEnemyDeath();
        }

        private void OnCollisionMessage(MonoBehaviour collisionBehaviour = null)
        {
            SIHelpers.SISimpleLogger(this, gameObject.name + " - collision detected ", SimpleLoggerTypes.Log);
        }
    }
}