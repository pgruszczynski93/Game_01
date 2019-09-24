using System;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public class GridMovementSettings
    {
        [Range(1, 10)] public float initialMovementSpeed;
        [Range(1, 10)] public float initialSpeedMultiplier;
        [Range(0, 5)] public float newWaveInitialSpeedChange;
        [Range(0, 1)] public float gridDownStep;
        [Range(0, 2)] public float speedMultiplierStep;
        [Range(0, 1)] public float smoothMovementStep;
        [Range(0, 1)] public float enemyWidthOffset;
        [Range(0f, 5f)] public float initialMovementSpeedMin;
        [Range(5f, 10f)] public float initialMovementSpeedMax;
        [Range(0f, 5f)] public float movementSpeedMin;
        [Range(5f, 10f)] public float movementSpeedMax;
    }
}