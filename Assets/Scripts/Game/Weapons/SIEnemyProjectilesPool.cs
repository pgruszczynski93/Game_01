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
    
    void SubscribeEvents() {
        SIEnemyGridEvents.OnShotInvoked += HandleOnShotInvoked;
        SIGameplayEvents.OnEnemyProjectilesCountChanged += HandleOnProjectilesCountChanged;
    }

    void UnsubscribeEvents() {
        SIEnemyGridEvents.OnShotInvoked -= HandleOnShotInvoked;
        SIGameplayEvents.OnEnemyProjectilesCountChanged -= HandleOnProjectilesCountChanged;
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
    
    //TESTING METHODS
    [Button]
    void TestWeaponTierUpdate(int availableProjectiles) {
        SIGameplayEvents.BroadcastOnEnemyProjectilesCountChanged(availableProjectiles);
    }
    //remove it later
    IEnumerator TierTester() {
        while (true) {
            yield return WaitUtils.WaitForCachedSeconds(3f);
            //Note: 1-4 because of array indexing => 0-3
            TestWeaponTierUpdate(Random.Range(1, 4));
        }
    }
    //
}
