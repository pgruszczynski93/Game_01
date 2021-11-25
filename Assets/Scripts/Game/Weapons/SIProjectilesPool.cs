using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders {
    public class SIProjectilesPool : SIObjectPool<SIProjectileEntity> {
        
        [SerializeField] protected SIProjectileSetup[] _availableProjectiles;

        protected int _availableProjectilesCount;
        protected int _currentSlotIndex;
        protected Transform[] _currentSlotSet;

        protected void HandleOnProjectilesCountChanged(int _availableProjectiles) {
            _availableProjectilesCount = _availableProjectiles;
        }

        protected override void ManagePooledObject() {
            Transform slotIndexTransform = _currentSlotSet[_currentSlotIndex];
            //Note 1: Always enable element from pool.
            _currentObjectFromPool.gameObject.SetActive(true); 
            _currentObjectFromPool.SetSpawnPosition(slotIndexTransform.position);
            //Note 2: Check projectiles parent upwards normalized rotation (direction of release force).
            _currentObjectFromPool.SetSpawnRotation(slotIndexTransform.up.normalized);
            _currentObjectFromPool.UseObjectFromPool();
        }
        
#if UNITY_EDITOR
        //Todo: dodać logikę, ktora poza zmianą poziomu broni (ilosć slotów) dodatkowo podmenia mesh i ustawia projectile.x
        protected override void AssignPoolableObjects() {
            base.AssignPoolableObjects();

            SIProjectileEntity currProjectile;
            Transform thisTransform = transform;
            for (var i = 0; i < _poolCapacity; i++) {
                currProjectile = _objectsPool[i];
                currProjectile.SetupProjectile(_availableProjectiles[_availableProjectilesCount]);
                currProjectile.SetParent(thisTransform);
            }
        }
    
#endif
    }
    
}