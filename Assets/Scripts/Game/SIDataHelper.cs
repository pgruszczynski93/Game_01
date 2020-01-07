using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceInvaders
{
    [Serializable]
    public class SIShaderModifierEffectInfo
    {
        public float effectIntensity;
        public float effectMaxValue;
        public float effectSpeed;
    }

    [Serializable]
    public class SICollectibleStatistics
    {
        public int gainedHealth;
        public int gainedScore;
        public float durationTime;
    }

    [Serializable]
    public class SIBonusInfo
    {
        public BonusType bonusType;
        public UnityEvent OnBonusFinishEvent;
    }
}