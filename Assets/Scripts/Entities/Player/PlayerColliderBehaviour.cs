using System;
using PG.Game.Collisions;
using PG.Game.Features;

namespace PG.Game.Entities.Player {
    public class PlayerColliderBehaviour : ColliderBehaviour, ICanCollide {
        public bool IsCollisionTriggered { get; set; }
        public Action<CollisionInfo> OnCollisionDetected { get; set; }

        public CollisionInfo GetCollisionInfo() {
            return _thisCollisionInfo;
        }

        protected override void AssignEvents() {
            base.AssignEvents();
            OnCollisionDetected += HandleOnCollisionDetected;
        }

        protected override void RemoveEvents() {
            base.RemoveEvents();
            OnCollisionDetected -= HandleOnCollisionDetected;
        }

        protected override void HandleOnCollisionDetected(CollisionInfo collisionInfo) {
            DetectPlayerHit();
            // TryDetectExplosiveHit(collisionInfo.collisionTag);
        }

        void DetectPlayerHit() {
//            Debug.Log("Player got hit.");
        }
    }
}