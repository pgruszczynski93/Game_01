using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyColliderBehaviour : SIColliderBehaviour, ICanCollide
    {
        public bool IsCollisionTriggered { get; set; }
        public Action<CollisionInfo> OnCollisionDetected { get; set; }
        public CollisionInfo GetCollisionInfo()
        {
            return _thisCollisionInfo;
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
        
        protected override void HandleOnCollisionDetected(CollisionInfo collisionInfo)
        {
            switch (collisionInfo.collisionTag)
            {
                case CollisionTag.Player:
                    //ToDo: Touching player doesn't take damage right now
                    // Debug.Log("HITTED BY PLAYER", this);s
                    break;
                case CollisionTag.PlayerWeapon:
                    // Debug.Log("HITTED BY PlayerWeapon", this);
                    break;
            }
            TryDetectExplosiveHit(collisionInfo.collisionTag);
        }
    }
}