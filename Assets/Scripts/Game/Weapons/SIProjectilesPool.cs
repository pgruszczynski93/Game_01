using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders {
    public class SIProjectilesPool : SIObjectPool<SIProjectileEntity> {
        
        [SerializeField] protected SIProjectileSetup[] _availableProjectiles;

        protected int _projectilesTier;
        protected int _currentSlotIndex;
        protected Transform[] _currentSlotSet;

        protected override void ManagePooledObject() {
            Transform slotIndex = _currentSlotSet[_currentSlotIndex];
            //Note 1: Always enable element from pool.
            _currentObjectFromPool.gameObject.SetActive(true); 
            _currentObjectFromPool.SetSpawnPosition(slotIndex.position);
            //Note 2: Check projectiles parent upwards rotation.
            _currentObjectFromPool.SetSpawnRotation(slotIndex.up);
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
                currProjectile.SetupProjectile(_availableProjectiles[_projectilesTier]);
                currProjectile.SetParent(thisTransform);
            }
        }
    
#endif
    }
    
}