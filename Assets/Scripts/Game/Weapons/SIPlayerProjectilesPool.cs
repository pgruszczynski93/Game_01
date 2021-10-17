using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class SIPlayerProjectilesPool: SIProjectilesPool {

        [SerializeField] SIPlayerShootController _playerShootController;
        
        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();

        void SubscribeEvents() {
            SIGameplayEvents.OnPlayerShoot += HandleOnPlayerShoot;
            SIGameplayEvents.OnPlayerProjectilesCountChanged += HandleOnProjectilesCountChanged;
        }

        void UnsubscribeEvents() {
            SIGameplayEvents.OnPlayerShoot -= HandleOnPlayerShoot;
            SIGameplayEvents.OnPlayerProjectilesCountChanged -= HandleOnProjectilesCountChanged;
        }

        void HandleOnPlayerShoot() {
            if (!_playerShootController.IsShootingEnabled)
                return;
            
            _currentSlotSet = _playerShootController.GetProjectileSlotsParent();
            _currentSlotIndex = 0;
            for (int i = 0; i < _currentSlotSet.Length; i++) {
                UpdatePool();
                _currentSlotIndex++;
            }
        }
        
        //TESTING METHODS
        [Button]
        void TestWeaponTierUpdate(int availableProjectiles) {
            SIGameplayEvents.BroadcastOnPlayerProjectilesCountChanged(availableProjectiles);
        }
    }
}