using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class SIPlayerProjectilesPool : SIProjectilesPool {

        [SerializeField] SIPlayerShootController _playerShootController;

        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            SIGameplayEvents.OnPlayerShoot += HandleOnPlayerShoot;
            SIGameplayEvents.OnPlayerProjectilesCountChanged += HandleOnProjectilesCountChanged;
        }

        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            SIGameplayEvents.OnPlayerShoot -= HandleOnPlayerShoot;
            SIGameplayEvents.OnPlayerProjectilesCountChanged -= HandleOnProjectilesCountChanged;
        }

        void HandleOnPlayerShoot() {
            if (!_playerShootController.IsShootingEnabled)
                return;
            
            _currentSlotSet = _playerShootController.GetProjectileSlotsParent();
            _currentSlotIndex = 0;
            for (int i = 0; i < _currentSlotSet.Length; i++) {
                SetNextObjectFromPool();
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