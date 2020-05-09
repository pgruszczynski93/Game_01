using System;

namespace SpaceInvaders
{
    public class SIEnemyGridEvents
    {
        public static event Action OnGridReset;
        public static event Action OnGridShootingReset;
        public static event Action<int> OnUpdateGridMovementSpeedTier;
        public static event Action<SIEnemyShootBehaviour> OnSubscribeToShooting;
        public static event Action<SIEnemyShootBehaviour> OnShootOrderRequested;
        
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
        public static void BroadcastOnSubscribeToShooting(SIEnemyShootBehaviour enemyShootBehaviour)
        {
            OnSubscribeToShooting?.Invoke(enemyShootBehaviour);
        }
        
        public static void BroadcastOnShootOrderReceived(SIEnemyShootBehaviour enemyShootBehaviour)
        {
            OnShootOrderRequested?.Invoke(enemyShootBehaviour);
        }
    }
}