using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyColliderBehaviour : SIMainColliderBehaviour<SIEnemyBehaviour>
    {
        protected override void OnEnable()
        {
            onCollisionCallback += _colliderParentBehaviour.Death;
            onCollisionCallback += InvokeEnemyDeathCallback;
        }

        protected override void OnDisable()
        {
            onCollisionCallback -= _colliderParentBehaviour.Death;
            onCollisionCallback -= InvokeEnemyDeathCallback;
        }

        private void InvokeEnemyDeathCallback()
        {
            SIEventsHandler.OnEnemyDeath?.Invoke();
        }
    }
}
