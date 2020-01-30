using System;

namespace SpaceInvaders
{
    public class SIBonusesEvents
    {
        public static event Action<BonusSettings> OnBonusEnabled;
        public static event Action<BonusSettings> OnBonusDisabled;
        
        public static void BroadcastOnBonusEnabled(BonusSettings bonusSettings)
        {
            OnBonusEnabled?.Invoke(bonusSettings);
        }

        public static void BroadcastOnBonusDisabled(BonusSettings bonusSettings)
        {
            OnBonusDisabled?.Invoke(bonusSettings);
        }
    }
}