using System;

namespace SpaceInvaders
{
    public class SIShieldColliderBehaviour : SIColliderBehaviour, ICanCollide
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
        }
    }
}