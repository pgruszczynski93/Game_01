using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders {
    public class SIExplosionsVfxPool : SIObjectPool<SIVFXBehaviour> {
        
        Vector3 _showPosition;

        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            SIGameplayEvents.OnExplosiveObjectHit += HandleOnExplosiveObjectHit;
        }

        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            SIGameplayEvents.OnExplosiveObjectHit -= HandleOnExplosiveObjectHit;
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