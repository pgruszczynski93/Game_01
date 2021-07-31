using Sirenix.OdinInspector;
using SpaceInvaders;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

public class SIEnemyProjectilesPool : SIObjectPool<SIProjectileEntity> {

    [SerializeField] SIProjectileSetup[] _availableProjectiles;

    bool _isPoolReleasingProjectiles;
    int _currentSlotIndex;
    int _projectilesTier;
    Transform[] _currentSlotSet;
    void OnEnable() {
        SubscribeEvents();
    }

    void OnDisable() {
        UnsubscribeEvents();
    }

    //TEST
    [Button]
    void Tier1() {
        SIGameplayEvents.BroadcastOnEnemyWeaponTierUpdate(WeaponTier.Tier_0);
    }
    [Button]
    void Tier2() {
        SIGameplayEvents.BroadcastOnEnemyWeaponTierUpdate(WeaponTier.Tier_1);
    }
    [Button]

    void Tier3() {
        SIGameplayEvents.BroadcastOnEnemyWeaponTierUpdate(WeaponTier.Tier_2);
    }
    //
    
    void SubscribeEvents() {
        SIEnemyGridEvents.OnShotInvoked += HandleOnShotInvoked;
        SIGameplayEvents.OnEnemyWeaponTierUpdate += HandleOnEnemyWeaponTierUpdate;
    }

    void UnsubscribeEvents() {
        SIEnemyGridEvents.OnShotInvoked -= HandleOnShotInvoked;
        SIGameplayEvents.OnEnemyWeaponTierUpdate -= HandleOnEnemyWeaponTierUpdate;
    }

    void HandleOnShotInvoked(SIEnemyShootController shootController) {
        if (!shootController.CanShoot || _isPoolReleasingProjectiles)
            return;

        _isPoolReleasingProjectiles = true;
        _currentSlotSet =  shootController.GetProjectileSlotsParent();
        _currentSlotIndex = 0;
        for (int i = 0; i < _currentSlotSet.Length; i++) {
            UpdatePool();
            _currentSlotIndex++;
        }

        _isPoolReleasingProjectiles = false;
    }
    
    void HandleOnEnemyWeaponTierUpdate(WeaponTier weaponTier) {
        _projectilesTier = (int) weaponTier;
    }

    protected override void ManagePooledObject() {
        Transform slotIndex = _currentSlotSet[_currentSlotIndex];
        _currentObjectFromPool.gameObject.SetActive(true); // testing line
        _currentObjectFromPool.SetSpawnPosition(slotIndex.position);
        _currentObjectFromPool.SetSpawnRotation(slotIndex.forward);
        _currentObjectFromPool.UseObjectFromPool();
    }
    
    //Todo: dodać logikę, ktora poza zmianą poziomu broni (ilosć slotów) dodatkowo podmenia mesh i ustawia projectile.
    
#if UNITY_EDITOR
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
