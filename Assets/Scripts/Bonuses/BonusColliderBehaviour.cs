﻿using System;
using PG.Game.Collisions;
using PG.Game.EventSystem;
using PG.Game.Features;
using UnityEngine;

namespace PG.Game.Bonuses {
    public class BonusColliderBehaviour : ColliderBehaviour, ICanCollide {
        [SerializeField] BonusBehaviour _bonusBehaviour;
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
            // todo: consider adding bonuses to enemies: 
            // todo======================================

            BonusesEvents.BroadcastOnBonusCollected(_bonusBehaviour.GetBonusVariantSettings());
            _bonusBehaviour.TryRunBonusCollectedRoutine();
        }
    }
}