using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceInvaders {
    public class SIPlayerProjectilesPool: SIProjectilesPool {

        SiPlayerShootController _playerShootController;
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

        
        void SubscribeEvents() {
            SIGameplayEvents.OnPlayerShoot += HandleOnPlayerShoot;
            SIGameplayEvents.OnPlayerWeaponTierUpdate += HandleOnPlayerWeaponTierUpdate;
        }

        void UnsubscribeEvents() {
            SIGameplayEvents.OnPlayerShoot -= HandleOnPlayerShoot;
            SIGameplayEvents.OnPlayerWeaponTierUpdate -= HandleOnPlayerWeaponTierUpdate;
        }

        void HandleOnPlayerShoot(SiPlayerShootController playerShootController) {
            if (_playerShootController == null)
                _playerShootController = playerShootController;

            _currentSlotSet = _playerShootController.GetProjectileSlotsParent();
            _currentSlotIndex = 0;
            for (int i = 0; i < _currentSlotSet.Length; i++) {
                UpdatePool();
                _currentSlotIndex++;
            }
        }
        
        // polaczyc metody z obu puli
        void HandleOnPlayerWeaponTierUpdate(WeaponTier weaponTier) {
            _projectilesTier = (int) weaponTier;
        }

        protected override void ManagePooledObject() {
            base.ManagePooledObject();
        }
    }
}