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
        [Range(0, 2)] public float speedMultiplierUpdateTime;
        [Range(0, 1)]public float gridMovementSpeedUpStep;
        public float initialMovementEaseDuration;
        public float horizontalDownstepDuration;

        public Vector3 worldStartPosition;
        public Vector3 worldTargetPosition;
        public Ease initialMovementEaseType;
        public Ease horizontalDownstepEaseType;

    }
}