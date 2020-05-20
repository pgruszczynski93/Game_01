using System;

namespace SpaceInvaders
{
    public class SIBonusesEvents
    {
        public static event Action<SIBonusDropController> OnBonusDropRequested;
        public static event Action<BonusSettings> OnBonusCollected;
        public static event Action<BonusSettings> OnBonusEnabled;
        public static event Action<BonusSettings> OnBonusDisabled;
        
        public static void BroadcastOnBonusDropRequested(SIBonusDropController controller)
        {
            OnBonusDropRequested?.Invoke(controller);
        }
        
        public static void BroadcastOnBonusEnabled(BonusSettings bonusSettings)
        {
            OnBonusEnabled?.Invoke(bonusSettings);
        }

        public static void BroadcastOnBonusDisabled(BonusSettings bonusSettings)
        {
            OnBonusDisabled?.Invoke(bonusSettings);
        }
        
        public static void BroadcastOnBonusCollected(BonusSettings bonusSettings)
        {
            OnBonusCollected?.Invoke(bonusSettings);
        }
    }
}