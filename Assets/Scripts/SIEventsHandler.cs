using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEventsHandler
    {
        public static Action OnGameStarted = delegate { };
        public static Action OnGameFinished = delegate { };
        public static Action OnEnemiesRespawn = delegate { };
        public static Action OnObjectMovement = delegate { };
        public static Action OnPlayerShoot = delegate { };
        public static Action OnEnemyShoot = delegate { };
        public static Action OnPlayerDeath = delegate { };
        public static Action OnPlayerHit = delegate { };
        public static Action OnEnemyDeath = delegate { };
        public static Action OnWaveEnd = delegate { };
        public static Action<float> OnEnemySpeedMultiplierChanged = delegate { };
        public static Action<SIBonusInfo> OnBonusCollision = delegate { };
        public static Action<SIShootedEnemyInfo> OnSwitchShootableEnemy = delegate { };
    }
}   