using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEventsHandler
    {
        public static Action OnObjectMovement = delegate { };
        public static Action OnPlayerShoot = delegate { };
        public static Action OnPlayerDeath = delegate { };
        public static Action OnEnemyDeath = delegate { };
        public static Action<float> OnEnemySpeedMultiplierChanged = delegate { };
    }
}   