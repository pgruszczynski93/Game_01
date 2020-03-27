using System;
using UnityEngine;
using DG.Tweening;

namespace SpaceInvaders
{
    [Serializable]
    public class GridMovementSettings : MovementSettings
    {
        [Range(0, 5)] public float newWaveInitialSpeedChange;
        [Range(0, 1)] public float gridDownStep;
        [Range(0, 2)] public float speedMultiplierStep;
        [Range(0, 1)] public float enemyWidthOffset;
        public Vector3 worldStartPosition;
        public Vector3 worldTargetPosition;
        public Ease easeType;
        public float easeDuration;
    }
}