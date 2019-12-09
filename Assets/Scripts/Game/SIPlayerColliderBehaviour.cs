using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerColliderBehaviour : SIColliderBehaviour, ICanCollide
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
            DetectPlayerHit();
        }

        void DetectPlayerHit()
        {
            Debug.Log("Player got hit.");
        }

//        void OnBonusGained(MonoBehaviour collisionBehaviour = null)                
//        void OnBonusGained(MonoBehaviour collisionBehaviour = null)                
//        {
//            SIBonus bonus = collisionBehaviour as SIBonus;
//
//            if (bonus == null)
//                return;
//
//            SIEventsHandler.BroadcastOnBonusCollision(bonus.BonusInfo);
//        }
    }
}