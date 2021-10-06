using System;

namespace SpaceInvaders
{
    public class SIEnemyGridEvents
    {
        public static event Action<SIEnemyShootController> OnReadyToShoot;
        public static event Action<SIEnemyShootController> OnShotInvoked;
        
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