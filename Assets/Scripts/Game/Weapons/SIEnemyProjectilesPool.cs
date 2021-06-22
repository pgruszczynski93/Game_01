using SpaceInvaders;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

public class SIEnemyProjectilesPool : SIObjectPool<SIProjectileEntity> {

    [SerializeField] int _projectilesTier; //Should start from 0
    [SerializeField] SIProjectileSetup[] _availableProjectiles;


    Transform[] testSlots;
    void OnEnable() {
        SubscribeEvents();
        //add here code which handles enemy weapon tier update;
    }

    void OnDisable() {
        UnsubscribeEvents();
        //add here code which handles enemy weapon tier update;
    }

    void SubscribeEvents() {
        SIEnemyGridEvents.OnShotInvoked += HandleOnShotInvoked;
    }
    
    void UnsubscribeEvents() {
        SIEnemyGridEvents.OnShotInvoked -= HandleOnShotInvoked;
    }

    void HandleOnShotInvoked(SIEnemyShootController shootController) {
        if (!shootController.CanShoot)
            return;
        
        var projectilesRoot = shootController.ProjectilesRootController;
        var projectilesSlots = projectilesRoot.ProjectilesSlotsPositions;
        testSlots = projectilesSlots;
        //get child based on tier index eg: this transform contains childs which referes to bullet's positon and rotation
        // i should iterate through this transform and launch each projectile
    }

    void UpdateEnemyProjectilesTier() {
        //get next scriptableobject from availableprojectile and inject it's values
        //into spawned projectile
    }
    protected override void ManagePooledObject() {
        _currentObjectFromPool.SetSpawnPosition(testSlots[0].position);
        _currentObjectFromPool.UseObjectFromPool();
    }
    
#if UNITY_EDITOR
    protected override void AssignPoolableObjects() {
        base.AssignPoolableObjects();

        SIProjectileEntity currProjectile;
        for (var i = 0; i < _poolCapacity; i++) {
            currProjectile = _objectsPool[i];
            currProjectile.SetupProjectile(_availableProjectiles[_projectilesTier]);
        }
    }
#endif
}
