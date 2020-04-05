using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyColliderBehaviour : SIColliderBehaviour, ICanCollide
    {
        public Action OnCollisionDetected { get; set; }
        public CollisionTag GetCollisionTag()
        {
            return _collisionTag;
        }

        protected override void AssignEvents()
        {
            base.AssignEvents();
            OnCollisionDetected += HandleOnCollisionDetected;
        }

        protected override void RemoveEvents()
        {
            base.RemoveEvents();
            OnCollisionDetected -= HandleOnCollisionDetected;
        }

        protected override void HandleOnCollisionDetected()
        {
            DetectHit();
        }

        void DetectHit()
        {
            //it should be managed by statistics- damage handling
            SIEventsHandler.BroadcastOnEnemyDeath();
        }
    }
}