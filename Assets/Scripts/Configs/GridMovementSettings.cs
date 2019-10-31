using System;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public class GridMovementSettings : MovementSettings
    {
        [Range(0, 5)] public float newWaveInitialSpeedChange;
        [Range(0, 1)] public float gridDownStep;
        [Range(0, 2)] public float speedMultiplierStep;
        [Range(0, 1)] public float enemyWidthOffset;
    }
}