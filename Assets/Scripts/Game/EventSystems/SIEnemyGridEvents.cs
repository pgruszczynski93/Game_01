using System;

namespace SpaceInvaders
{
    public class SIEnemyGridEvents
    {
        public static event Action OnGridStarted;
        public static event Action OnGridReset;
        public static event Action OnGridShootingStarted;
        public static event Action OnGridShootingStopped;

        public static void BroadcastOnGridStarted()
        {
            OnGridStarted?.Invoke();
        }

        public static void BroadcastOnGridReset()
        {
            OnGridReset?.Invoke();
        }
        
        public static void BroadcastOnGridShootingPossible()
        {
            OnGridShootingStarted?.Invoke();
        }
        
        public static void BroadcastOnGridShootingStopped()
        {
            OnGridShootingStopped?.Invoke();
        }
    }
}