using System;
using PG.Game.Entities.Enemies;

namespace PG.Game.EventSystem {
    public class EnemyGridEvents {
        public static event Action<bool> OnGridOnGridVisibilityChanged;
        public static event Action<EnemyShootController> OnReadyToShoot;
        public static event Action<EnemyShootController> OnShotInvoked;

        public static void BroadcastOnGridOnGridVisibilityChanged(bool isVisible) {
            OnGridOnGridVisibilityChanged?.Invoke(isVisible);
        }

        public static void BroadcastOnReadyToShoot(EnemyShootController enemyShootController) {
            OnReadyToShoot?.Invoke(enemyShootController);
        }

        public static void BroadcastOnShotInvoked(EnemyShootController enemyShootController) {
            OnShotInvoked?.Invoke(enemyShootController);
        }
    }
}