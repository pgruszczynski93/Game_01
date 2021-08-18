using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class SIPlayerProjectilesPool: SIProjectilesPool {

        [SerializeField] SIPlayerShootController _playerShootController;
        
        void OnEnable() {
            SubscribeEvents();
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
        
        [Button]
        void TestWeaponTierUpdate(WeaponTier tier) {
            SIGameplayEvents.BroadcastOnPlayerWeaponTierUpdate(tier);
        }
    }
}