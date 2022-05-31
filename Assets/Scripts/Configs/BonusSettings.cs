using System;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        public bool isBonusTaskActive;
        public UniTask bonusTask;
        public CancellationTokenSource bonusCancellation;

        public RuntimeBonus(BonusSettings settings) : base(settings) {
            bonusCancellation = new CancellationTokenSource();
        }
    }

    [Serializable]
    public struct BonusDropInfo
    {
        [Range(0, 100)] public int minDropRate;
        [Range(0, 100)] public int maxDropRate;
    }
}