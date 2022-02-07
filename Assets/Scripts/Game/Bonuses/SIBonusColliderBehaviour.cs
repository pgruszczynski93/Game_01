using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIBonusColliderBehaviour : SIColliderBehaviour, ICanCollide
    {
        [SerializeField] SIBonus _bonusBehaviour;
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
            // todo: consider adding bonuses to enemies: 
            // todo======================================

            SIBonusesEvents.BroadcastOnBonusCollected(_bonusBehaviour.GetBonusVariantSettings());
            _bonusBehaviour.TryRunBonusCollectedRoutine();
        }
    }
}