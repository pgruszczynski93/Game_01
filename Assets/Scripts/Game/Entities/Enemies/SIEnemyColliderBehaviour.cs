using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyColliderBehaviour : SIColliderBehaviour, ICanCollide
    {
        [SerializeField] SIEnemyBehaviour _enemyBehaviour;
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
//                    Debug.Log("HITTED BY PLAYER", this);
                    break;
                case CollisionTag.PlayerWeapon:
//                    Debug.Log("HITTED BY PlayerWeapon", this);
                    break;
            }
            
            TryDetectExplosiveHit(collisionInfo.collisionTag);
        }
    }
}