using PG.Game.Configs;
using PG.Game.Features.ObjectsPool;
using PG.Game.Weapons.Projectile;
using UnityEngine;

namespace PG.Game.Weapons {
    public class ProjectilesPool : ObjectsPool<ProjectileBehaviour> {
        [SerializeField] protected SIProjectileSetup[] _availableProjectiles;

        protected int _availableProjectilesCount;
        protected int _currentSlotIndex;
        protected Transform[] _currentSlotSet;

        protected void HandleOnProjectilesCountChanged(int _availableProjectiles) {
            _availableProjectilesCount = _availableProjectiles;
        }

        protected override void ManagePoolableObject() {
            Transform slotIndexTransform = _currentSlotSet[_currentSlotIndex];
            //Note 1: Always enable element from pool.
            _currentlyPooledObject.gameObject.SetActive(true);
            _currentlyPooledObject.SetSpawnPosition(slotIndexTransform.position);
            //Note 2: Check projectiles parent upwards normalized rotation (direction of release force).
            _currentlyPooledObject.SetSpawnRotation(slotIndexTransform.up.normalized);
            _currentlyPooledObject.PerformOnPoolActions();
        }

#if UNITY_EDITOR
        //Todo: dodać logikę, ktora poza zmianą poziomu broni (ilosć slotów) dodatkowo podmenia mesh i ustawia projectile.x
        protected override void AssignPoolableObjects() {
            base.AssignPoolableObjects();

            ProjectileBehaviour currProjectile;
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