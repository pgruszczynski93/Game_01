using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class SIPlayerProjectilesPool: SIProjectilesPool {

        [SerializeField] SIPlayerShootController _playerShootController;
        
        void OnEnable() {
            SubscribeEvents();
            StartCoroutine(TierTester());
        }

        void OnDisable() {
            UnsubscribeEvents();
        }

        void SubscribeEvents() {
            SIGameplayEvents.OnPlayerShoot += HandleOnPlayerShoot;
            SIGameplayEvents.OnPlayerWeaponTierUpdate += HandleOnWeaponTierUpdate;
        }

        void UnsubscribeEvents() {
            SIGameplayEvents.OnPlayerShoot -= HandleOnPlayerShoot;
            SIGameplayEvents.OnPlayerWeaponTierUpdate -= HandleOnWeaponTierUpdate;
        }

        void HandleOnPlayerShoot() {
            _currentSlotSet = _playerShootController.GetProjectileSlotsParent();
            _currentSlotIndex = 0;
            for (int i = 0; i < _currentSlotSet.Length; i++) {
                UpdatePool();
                _currentSlotIndex++;
            }
        }
        
        //TESTING METHODS
        [Button]
        void TestWeaponTierUpdate(WeaponTier tier) {
            SIGameplayEvents.BroadcastOnPlayerWeaponTierUpdate(tier);
        }
        //remove it later
        IEnumerator TierTester() {
            while (true) {
                yield return WaitUtils.WaitForCachedSeconds(3f);
                TestWeaponTierUpdate((WeaponTier) Random.Range(0, 3));
            }
        }
        //
    }
}