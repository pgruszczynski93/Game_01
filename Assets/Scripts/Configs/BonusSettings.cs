using System;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public struct BonusSettings
    {
        public BonusType bonusType;
        public BonusProperties bonusProperties;
    }

    [Serializable]
    public struct BonusProperties
    {
        public int gainedHealth;
        public int gainedScore;
        public int bonusLevel;
        public float durationTime;
        public float releaseForceMultiplier;
        public Coroutine bonusRoutine;
    }
}