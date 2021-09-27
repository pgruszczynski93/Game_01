using System;
using UnityEngine;

namespace SpaceInvaders
{
    public static class SIGameplayEvents {
        public static event Action OnEnemyWeaponHit;
        public static event Action OnWaveStart;
        public static event Action OnWaveEnd;
        public static event Action OnPlayerShoot;
        public static event Action<Vector3> OnExplosiveObjectHit;
        public static event Action<DamageInfo> OnDamage;
        public static event Action<SIEnemyBehaviour> OnEnemyDeath;
        public static event Action<WeaponTier> OnEnemyWeaponTierUpdate;
        public static event Action<WeaponTier> OnPlayerWeaponTierUpdate;
//        public static event Action<float> OnEnemySpeedMultiplierChanged;

        public static void BroadcastOnEnemyWeaponHit()
        {
            OnEnemyWeaponHit?.Invoke();
        }
        public static void BroadcastOnDamage(DamageInfo damageInfo)
        {
            OnDamage?.Invoke(damageInfo);
        }

        public static void BroadcastOnEnemyDeath(SIEnemyBehaviour enemyBehaviour)
        {
            OnEnemyDeath?.Invoke(enemyBehaviour);
        }

        public static void BroadcastOnPlayerWeaponTierUpdate(WeaponTier weaponTier)
        {
            OnPlayerWeaponTierUpdate?.Invoke(weaponTier);
        }
        
        public static void BroadcastOnEnemyWeaponTierUpdate(WeaponTier weaponTier)
        {
            OnEnemyWeaponTierUpdate?.Invoke(weaponTier);
        }
        
        public static void BroadcastOnPlayerShoot()
        {
            OnPlayerShoot?.Invoke();
        }
        
        public static void BroadcastOnWaveStart() {
            OnWaveStart?.Invoke();
        }
        
        public static void BroadcastOnWaveEnd()
        {
            OnWaveEnd?.Invoke();
        }

        public static void BroadcastOnExplosiveObjectHit(Vector3 collisionPos) {
            OnExplosiveObjectHit?.Invoke(collisionPos);
        }

        
        //        public static void BroadcastOnEnemySpeedMultiplierChanged(float multiplier)
//        {
//            OnEnemySpeedMultiplierChanged?.Invoke(multiplier);
//        }
    }
}