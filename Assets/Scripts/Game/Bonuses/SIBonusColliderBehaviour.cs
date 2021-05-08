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
            // BonusSettings bonusSettings = _bonusBehaviour.GetBonusSettings();
            // Debug.Log($"[SIBonusColliderBehaviour] Bonus received {bonusSettings.bonusDropInfo.bonusType}");
            SIBonusesEvents.BroadcastOnBonusCollected(_bonusBehaviour.BonusVariantSettings);
            _bonusBehaviour.TryRunBonusCollectedRoutine();
        }
    }
}