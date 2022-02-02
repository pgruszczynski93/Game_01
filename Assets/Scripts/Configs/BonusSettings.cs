using System;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public class BonusSettings
    {
        public BonusType bonusType;
        public int gainedHealth;
        public int gainedScore;
        public int bonusLevel;
        public float durationTime;

        public BonusSettings() { }

        public BonusSettings(BonusSettings settings) {
            bonusType = settings.bonusType;
            gainedHealth = settings.gainedHealth;
            gainedScore = settings.gainedScore;
            bonusLevel = settings.bonusLevel;
            durationTime = settings.durationTime;
        }
    }

    [Serializable]
    public class RuntimeBonus : BonusSettings {
        public bool isCoroutineActive;
        public Coroutine bonusRoutine;
        public RuntimeBonus(BonusSettings settings) : base(settings) {}
    }

    [Serializable]
    public struct BonusDropInfo
    {
        [Range(0, 100)] public int minDropRate;
        [Range(0, 100)] public int maxDropRate;
    }
}