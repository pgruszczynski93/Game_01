using System;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public struct BonusSettings
    {
        public BonusProperties bonusProperties;
        public BonusDropInfo bonusDropInfo;
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

    [Serializable]
    public struct BonusDropInfo
    {
        public BonusType bonusType;
        [Range(0, 100)] public int minDropRate;
        [Range(0, 100)] public int maxDropRate;
    }
}