using System;

namespace SpaceInvaders
{
    public static class SIGameplayEvents {
        public static event Action OnEnemyWeaponHit;
        public static event Action<DamageInfo> OnDamage;

        public static void BroadcastOnEnemyWeaponHit()
        {
            OnEnemyWeaponHit?.Invoke();
        }
        public static void BroadcastOnDamage(DamageInfo damageInfo)
        {
            OnDamage?.Invoke(damageInfo);
        }
        
        
    }
}