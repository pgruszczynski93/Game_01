using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEventsHandler
    {
        public static event Action OnGameStarted;
        public static event Action OnGameFinished;
        public static event Action OnSpawnObject;
        public static event Action OnGameQuit;
        public static event Action OnEnemiesRespawn;
        public static event Action OnNonPlayableUpdate;
        public static event Action OnUpdate;
        public static event Action OnShadersUpdate;
        public static event Action OnEnemyDeath;
        public static event Action OnDebugInputHandling;
        public static event Action OnWaveEnd;
        public static event Action OnShootInputReceived;
        public static event Action<float> OnEnemySpeedMultiplierChanged;
        public static event Action<Vector3> OnAxesInputReceived;
        public static event Action<int> OnShootingEnemiesUpdate;
        public static event Action<SIBonusInfo> OnBonusCollision;

        public static void BroadcastOnShootInputReceived()
        {
            OnShootInputReceived?.Invoke();
        }

        public static void BroadcastOnAxesInputReceived(Vector3 inputVector)
        {
            OnAxesInputReceived?.Invoke(inputVector);
        }

        public static void BroadcastOnShootingEnemiesUpdate(int index)
        {
            OnShootingEnemiesUpdate?.Invoke(index);
        }

        public static void BroadcastOnSpawnObject()
        {
            OnSpawnObject?.Invoke();
        }

        public static void BroadcastOnGameStarted()
        {
            OnGameStarted?.Invoke();
        }

        public static void BroadcastOnGameFinished()
        {
            OnGameFinished?.Invoke();
        }

        public static void BroadcastOnNonPlayableUpdate()
        {
            OnNonPlayableUpdate?.Invoke();
        }

        public static void BroadcastOnGameQuit()
        {
            OnGameQuit?.Invoke();
        }

        public static void BroadcastOnEnemiesRespawn()
        {
            OnEnemiesRespawn?.Invoke();
        }
        
        public static void BroadcastOnUpdate()
        {
            OnUpdate?.Invoke();
        }
        
        public static void BroadcastOnShadersUpdate()
        {
            OnShadersUpdate?.Invoke();
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
        
    }
}   