using PG.Game.Entities.Player;
using PG.Game.EventSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PG.Game.Weapons {
    public class PlayerProjectilesPool : ProjectilesPool {
        [SerializeField] PlayerShootController _playerShootController;

        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            GameplayEvents.OnPlayerShoot += HandleOnPlayerShoot;
            GameplayEvents.OnPlayerProjectilesCountChanged += HandleOnProjectilesCountChanged;
        }

        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            GameplayEvents.OnPlayerShoot -= HandleOnPlayerShoot;
            GameplayEvents.OnPlayerProjectilesCountChanged -= HandleOnProjectilesCountChanged;
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
            GameplayEvents.BroadcastOnPlayerProjectilesCountChanged(availableProjectiles);
        }
    }
}