using System;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public class BonusSettings
    {
        public BonusType bonusType;
        public BonusProperties bonusProperties;
    }

    [Serializable]
    public class BonusProperties
    {
        public bool isBonusActive;
        public int gainedHealth;
        public int gainedScore;
        public int bonusLevel;
        public float durationTime;
        public float releaseForceMultiplier;
        public Coroutine bonusRoutine;
    }
}