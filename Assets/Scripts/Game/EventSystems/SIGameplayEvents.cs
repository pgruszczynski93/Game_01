using System;

namespace SpaceInvaders
{
    public static class SIGameplayEvents
    {
        public static event Action<DamageInfo> OnDamage;

        public static void BroadcastOnDamage(DamageInfo damageInfo)
        {
            OnDamage?.Invoke(damageInfo);
        }
    }
}