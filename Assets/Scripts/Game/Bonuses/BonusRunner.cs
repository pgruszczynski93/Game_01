using System;
using System.Collections;

namespace SpaceInvaders
{
    public static class BonusRunner
    {
        public static IEnumerator RunBonus(BonusSettings bonusSettings)
        {
            bonusSettings.bonusProperties.isBonusActive = true;
            SIBonusesEvents.BroadcastOnBonusEnabled(bonusSettings.bonusType);
            yield return SIWaitUtils.WaitForCachedSeconds(bonusSettings.bonusProperties.durationTime);
            SIBonusesEvents.BroadcastOnBonusDisabled(bonusSettings.bonusType);
            bonusSettings.bonusProperties.isBonusActive = false;
        }
    }
}