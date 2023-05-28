using PG.Game.EventSystem;
using PG.Game.Features.ObjectsPool;
using PG.Game.VFX;
using UnityEngine;

namespace PG.Game.Weapons {
    public class ExplosionsVfxPool : ObjectsPool<ExplosionVFX> {
        Vector3 _showPosition;

        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            GameplayEvents.OnExplosiveObjectHit += HandleOnExplosiveObjectHit;
        }

        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            GameplayEvents.OnExplosiveObjectHit -= HandleOnExplosiveObjectHit;
        }

        void HandleOnExplosiveObjectHit(Vector3 collisionPos) {
            _showPosition = collisionPos;
            SetNextObjectFromPool();
        }

        protected override void ManagePoolableObject() {
            _currentlyPooledObject.SetSpawnPosition(_showPosition);
            _currentlyPooledObject.PerformOnPoolActions();
        }
    }
}