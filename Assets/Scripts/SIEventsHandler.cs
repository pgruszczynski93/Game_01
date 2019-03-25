using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEventsHandler
    {
        public static event Action OnGameStarted = delegate { };
        public static event Action OnGameFinished = delegate { };
        public static event Action OnGameQuit = delegate { };
        public static event Action OnEnemiesRespawn = delegate { };
        public static event Action OnObjectsMovement = delegate { };
        public static event Action OnShadersUpdate = delegate { };
        public static event Action OnPlayerShoot = delegate { };
        public static event Action OnEnemyShoot = delegate { };
        public static event Action OnPlayerDeath = delegate { };
        public static event Action OnPlayerHit = delegate { };
        public static event Action OnEnemyDeath = delegate { };
        public static event Action OnDebugInputHandling = delegate { };
        public static event Action OnWaveEnd = delegate { };
        public static event Action<float> OnEnemySpeedMultiplierChanged = delegate { };
        public static event Action<SIBonusInfo> OnBonusCollision = delegate { };
        public static event Action<SIShootedEnemyInfo> OnSwitchShootableEnemy = delegate { };
        public static void BroadcastOnGameStarted()
        {
            OnGameStarted?.Invoke();
        }

        public static void BroadcastOnGameFinished()
        {
            OnGameFinished?.Invoke();
        }

        public static void BroadcastOnGameQuit()
        {
            OnGameQuit?.Invoke();
        }

        public static void BroadcastOnEnemiesRespawn()
        {
            OnEnemiesRespawn?.Invoke();
        }
        
        public static void BroadcastOnObjectsMovement()
        {
            OnObjectsMovement?.Invoke();
        }
        
        public static void BroadcastOnShadersUpdate()
        {
            OnShadersUpdate?.Invoke();
        }
        
        public static void BroadcastOnPlayerShoot()
        {
            OnPlayerShoot?.Invoke();
        }
        
        public static void BroadcastOnEnemyShoot()
        {
            OnEnemyShoot?.Invoke();
        }
        
        public static void BroadcastOnPlayerDeath()
        {
            OnPlayerDeath?.Invoke();
        }
        
        public static void BroadcastOnPlayerHit()
        {
            OnPlayerHit?.Invoke();
        }
        
        public static void BroadcastOnEnemyDeath()
        {
            OnEnemyDeath?.Invoke();
        }
        
        public static void BroadcastOnDebugInputHandling()
        {
            OnDebugInputHandling?.Invoke();
        }
        
        public static void BroadcastOnWaveEnd()
        {
            OnWaveEnd?.Invoke();
        }

        public static void BroadcastOnEnemySpeedMultiplierChanged(float multiplier)
        {
            OnEnemySpeedMultiplierChanged?.Invoke(multiplier);
        }
        
        public static void BroadcastOnBonusCollision(SIBonusInfo bonusInfo)
        {
            OnBonusCollision?.Invoke(bonusInfo);
        }
        
        public static void BroadcastOnSwitchShootableEnemy(SIShootedEnemyInfo enemyInfo)
        {
            OnSwitchShootableEnemy?.Invoke(enemyInfo);
        }
        
        
    }
}   