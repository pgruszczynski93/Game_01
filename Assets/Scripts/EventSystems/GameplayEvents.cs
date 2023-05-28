using System;
using PG.Game.Collisions;
using PG.Game.Systems.WaveSystem;
using PG.Game.Entities.Enemies;
using PG.Game.Features;
using UnityEngine;

namespace PG.Game.EventSystem {
    public static class GameplayEvents {
        public static event Action OnEnemyWeaponHit;
        public static event Action<WaveType> OnWaveStart;
        public static event Action<WaveType> OnWaveEnd;
        public static event Action OnWaveCoolDown;
        public static event Action OnPlayerShoot;

        //This event runs only in Start method of object which speed can be modified with special manager.
        public static event Action<IModifyTimeSpeedMultiplier> OnSpeedModificationRequested;
        public static event Action<Vector3> OnExplosiveObjectHit;
        public static event Action<DamageInfo> OnDamage;
        public static event Action<EnemyBehaviour> OnEnemyDeath;
        public static event Action<int> OnEnemyProjectilesCountChanged;
        public static event Action<int> OnPlayerProjectilesCountChanged;

        public static void BroadcastOnEnemyWeaponHit() {
            OnEnemyWeaponHit?.Invoke();
        }

        public static void BroadcastOnSpeedModificationRequested(IModifyTimeSpeedMultiplier behaviourToModifyTimeSpeedMultiplier) {
            OnSpeedModificationRequested?.Invoke(behaviourToModifyTimeSpeedMultiplier);
        }

        public static void BroadcastOnDamage(DamageInfo damageInfo) {
            OnDamage?.Invoke(damageInfo);
        }

        public static void BroadcastOnEnemyDeath(EnemyBehaviour enemyBehaviour) {
            OnEnemyDeath?.Invoke(enemyBehaviour);
        }

        public static void BroadcastOnPlayerProjectilesCountChanged(int projectilesCount) {
            OnPlayerProjectilesCountChanged?.Invoke(projectilesCount);
        }

        public static void BroadcastOnEnemyProjectilesCountChanged(int projectilesCount) {
            OnEnemyProjectilesCountChanged?.Invoke(projectilesCount);
        }

        public static void BroadcastOnPlayerShoot() {
            OnPlayerShoot?.Invoke();
        }

        public static void BroadcastOnWaveStart(WaveType waveType) {
            OnWaveStart?.Invoke(waveType);
        }

        public static void BroadcastOnWaveEnd(WaveType waveType) {
            OnWaveEnd?.Invoke(waveType);
        }

        public static void BroadcastOnWaveCoolDown() {
            OnWaveCoolDown?.Invoke();
        }

        public static void BroadcastOnExplosiveObjectHit(Vector3 collisionPos) {
            OnExplosiveObjectHit?.Invoke(collisionPos);
        }
    }
}