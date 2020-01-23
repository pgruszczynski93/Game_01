using System;
using System.Collections;

namespace SpaceInvaders
{
    public static class BonusRunner
    {
        public static IEnumerator RunBonus(float durationTime, Action onBonusStarted, Action onBonusFinished)
        {
            onBonusStarted?.Invoke();
            yield return SIWaitUtils.WaitForCachedSeconds(durationTime);
            onBonusFinished?.Invoke();
        }
    }
}