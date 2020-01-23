using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIBonusColliderBehaviour : SIColliderBehaviour, ICanCollide
    {
        [SerializeField] SIBonus _bonusBehaviour;
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
            BonusSettings bonusSettings = _bonusBehaviour.GetBonusSettings();
            Debug.Log($"[SIBonusColliderBehaviour] Bonus received {bonusSettings.bonusType}");
            SIEventsHandler.BroadcastOnBonusCollected(bonusSettings);
        }
    }
}