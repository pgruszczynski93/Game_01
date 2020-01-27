using System;

namespace SpaceInvaders
{
    public class SIBonusesEvents
    {
        public static event Action<BonusType> OnBonusEnabled;
        public static event Action<BonusType> OnBonusDisabled;
        
        public static void BroadcastOnBonusEnabled(BonusType bonusType)
        {
            OnBonusEnabled?.Invoke(bonusType);
        }

        public static void BroadcastOnBonusDisabled(BonusType bonusType)
        {
            OnBonusDisabled?.Invoke(bonusType);
        }
    }
}