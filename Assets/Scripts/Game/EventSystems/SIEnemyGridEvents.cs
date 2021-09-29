using System;

namespace SpaceInvaders
{
    public class SIEnemyGridEvents
    {
        public static event Action OnGridObjectsReloaded;
        public static event Action OnUpdateGridMovementSpeedTier;
        public static event Action<SIEnemyShootController> OnReadyToShoot;
        public static event Action<SIEnemyShootController> OnShotInvoked;
        
        public static void BroadcastOnGridObjectsReloaded()
        {
            OnGridObjectsReloaded?.Invoke();
        }
        
        public static void BroadcastOnUpdateGridMovementSpeedTier()
        {
            OnUpdateGridMovementSpeedTier?.Invoke();
        }
        public static void BroadcastOnReadyToShoot(SIEnemyShootController enemyShootController)
        {
            OnReadyToShoot?.Invoke(enemyShootController);
        }
        
        public static void BroadcastOnShotInvoked(SIEnemyShootController enemyShootController)
        {
            OnShotInvoked?.Invoke(enemyShootController);
        }
    }
}