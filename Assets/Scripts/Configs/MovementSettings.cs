using System;
using UnityEngine;

namespace PG.Game.Configs {
    [Serializable]
    public class MovementSettings {
        [Range(1, 30)] public float initialMovementSpeed;
        [Range(1, 10)] public float initialSpeedMultiplier;
        [Range(0, 1)] public float movementSmoothStep;
        [Range(0, 100)] public float maxMovementSpeed;
        [Range(0f, 10f)] public float maxMovementSpeedMultiplier;
    }
}