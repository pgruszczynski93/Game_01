using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyColliderBehaviour : SIMainColliderBehaviour<SIEnemyBehaviour>
    {
        protected override void OnEnable()
        {
            for (int i = 0; i < _objectTags.Length; i++)
            {
                _onCollisionActions[_objectTags[i]] += _colliderParentBehaviour.Death;
                _onCollisionActions[_objectTags[i]] += InvokeEnemyDeathCallback;
            }
        }

        protected override void OnDisable()
        {
            for (int i = 0; i < _objectTags.Length; i++)
            {
                _onCollisionActions[_objectTags[i]] -= _colliderParentBehaviour.Death;
                _onCollisionActions[_objectTags[i]] -= InvokeEnemyDeathCallback;
            }
        }

        private void InvokeEnemyDeathCallback(MonoBehaviour collisionBehaviour = null)
        {
            SIEventsHandler.BroadcastOnEnemyDeath();
        }
    }
}