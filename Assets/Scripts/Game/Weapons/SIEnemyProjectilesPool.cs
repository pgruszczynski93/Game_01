using System.Collections;
using Sirenix.OdinInspector;
using SpaceInvaders;
using UnityEngine;

public class SIEnemyProjectilesPool : SIProjectilesPool {

    bool _isPoolReleasingProjectiles;
    
    void OnEnable() {
        SubscribeEvents();
        StartCoroutine(TierTester());
    }

    void OnDisable() {
        UnsubscribeEvents();
    }

    //TESTING METHODS
    [Button]
    void TestWeaponTierUpdate(WeaponTier tier) {
        SIGameplayEvents.BroadcastOnEnemyWeaponTierUpdate(tier);
    }
    //remove it later
    IEnumerator TierTester() {
        while (true) {
            yield return WaitUtils.WaitForCachedSeconds(3f);
            TestWeaponTierUpdate((WeaponTier) Random.Range(0, 3));
        }
    }
    //

    void SubscribeEvents() {
        SIEnemyGridEvents.OnShotInvoked += HandleOnShotInvoked;
        SIGameplayEvents.OnEnemyWeaponTierUpdate += HandleOnWeaponTierUpdate;
    }

    void UnsubscribeEvents() {
        SIEnemyGridEvents.OnShotInvoked -= HandleOnShotInvoked;
        SIGameplayEvents.OnEnemyWeaponTierUpdate -= HandleOnWeaponTierUpdate;
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
}
