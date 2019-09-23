using UnityEngine;

namespace SpaceInvaders
{
    public class GridMovementSettings
    {
        [Range(1, 10)] public float _initialMovementSpeed;
        public float initialMovementSpeedMax;
        public float initialMovementSpeedMin;
        public float movementSpeedMax;
        public float movementSpeedMin;
        [Range(1, 10)] public float _initialSpeedMultiplier;
        [Range(0, 5)] public float _newWaveInitialSpeedChange;
        [Range(0, 1)] public float _gridDownStep;
        [Range(0, 2)] public float _speedMultiplierStep;
        [Range(0, 1)] public float _smoothMovementStep;
    }
}