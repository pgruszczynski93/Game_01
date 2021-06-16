using System;

namespace SpaceInvaders
{
    public class SIEnemyGridEvents
    {
        public static event Action OnGridObjectsReloaded;
        public static event Action OnGridReset;
        public static event Action OnGridShootingReset;
        public static event Action<int> OnUpdateGridMovementSpeedTier;
        public static event Action<SIEnemyShootController> OnReadyToShoot;
        public static event Action<SIEnemyShootController> OnShotInvoked;
        
        public static void BroadcastOnGridObjectsReloaded()
        {
            OnGridObjectsReloaded?.Invoke();
        }
        
        public static void BroadcastOnGridReset()
        {
            OnGridReset?.Invoke();
        }
        
        public static void BroadcastOnGridOnGridShootingReset()
        {
            OnGridShootingReset?.Invoke();
        }

        public static void BroadcastOnUpdateGridMovementSpeedTier(int newTier)
        {
            OnUpdateGridMovementSpeedTier?.Invoke(newTier);
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