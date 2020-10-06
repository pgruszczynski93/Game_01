using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEventsHandler {
        public static event Action<GameStates> OnGameStateChanged;
        public static event Action OnNonPlayableUpdate;
        public static event Action OnUpdate;
        public static event Action<SIEnemyBehaviour> OnEnemyDeath;
        public static event Action OnWaveEnd;
        public static event Action OnPlayerShoot;
        public static event Action<WeaponTier> OnWeaponTierUpdate;
//        public static event Action<float> OnEnemySpeedMultiplierChanged;
        public static event Action<Vector3> OnAxesInputReceived;

        public static void BroadcastOnGameStateChanged(GameStates gameState)
        {
            OnGameStateChanged?.Invoke(gameState);
        }
        
        public static void BroadcastOnWeaponTierUpdate(WeaponTier weaponTier)
        {
            OnWeaponTierUpdate?.Invoke(weaponTier);
        }
        
        public static void BroadcastOnPlayerShoot()
        {
            OnPlayerShoot?.Invoke();
        }

        public static void BroadcastOnAxesInputReceived(Vector3 inputVector)
        {
            OnAxesInputReceived?.Invoke(inputVector);
        }

        public static void BroadcastOnNonPlayableUpdate()
        {
            OnNonPlayableUpdate?.Invoke();
        }

        public static void BroadcastOnUpdate()
        {
            OnUpdate?.Invoke();
        }
        
        public static void BroadcastOnEnemyDeath(SIEnemyBehaviour enemyBehaviour)
        {
            OnEnemyDeath?.Invoke(enemyBehaviour);
        }
        
        public static void BroadcastOnWaveEnd()
        {
            OnWaveEnd?.Invoke();
        }

//        public static void BroadcastOnEnemySpeedMultiplierChanged(float multiplier)
//        {
//            OnEnemySpeedMultiplierChanged?.Invoke(multiplier);
//        }
    }
}   