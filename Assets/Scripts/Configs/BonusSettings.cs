using System;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public struct BonusSettings
    {
        public BonusType bonusType;
        public int gainedHealth;
        public int gainedScore;
        public int bonusLevel;
        public float durationTime;
        public float releaseForceMultiplier;
        public Coroutine bonusRoutine;
    }

    [Serializable]
    public struct BonusDropInfo
    {
        [Range(0, 100)] public int minDropRate;
        [Range(0, 100)] public int maxDropRate;
    }
}