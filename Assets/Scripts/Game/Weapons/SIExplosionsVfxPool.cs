using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders {
    public class SIExplosionsVfxPool : SIObjectPool<SIVFXBehaviour> {
        
        Vector3 _showPosition;

        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();

        void SubscribeEvents() {
            SIGameplayEvents.OnExplosiveObjectHit += HandleOnExplosiveObjectHit;
        }

        void UnsubscribeEvents() {
            SIGameplayEvents.OnExplosiveObjectHit -= HandleOnExplosiveObjectHit;
        }
        
        void HandleOnExplosiveObjectHit(Vector3 collisionPos) {
            _showPosition = collisionPos;
            UpdatePool();
        }
        
        protected override void ManagePooledObject() {
            _currentObjectFromPool.SetSpawnPosition(_showPosition);
            _currentObjectFromPool.UseObjectFromPool();
        }

    }
}