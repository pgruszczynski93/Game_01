using SpaceInvaders;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

public class SIEnemyProjectilesPool : SIObjectPool<SIProjectileEntity> {

    [SerializeField] int _projectilesTier; //Should start from 0
    [SerializeField] SIProjectileSetup[] _availableProjectiles;


    void OnEnable() {
        //add here code which handles enemy weapon tier update;
    }

    void OnDisable() {
        //add here code which handles enemy weapon tier update;
    }


    void UpdateEnemyProjectilesTier() {
        //get next scriptableobject from availableprojectile and inject it's values
        //into spawned projectile
    }
    protected override void ManagePooledObject() {
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
