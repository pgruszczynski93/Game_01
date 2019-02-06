using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceInvaders
{
    [Serializable]
    public class SIStatistics
    {
        public int currentHealth;
        public int currentScore;
        public int currentWave;
        public WeaponType currentWeaponType;
    }

    [Serializable]
    public class SICollectibleStatistics
    {
        public int gainedHealth;
        public int gainedScore;
        public WeaponType gainedWeaponType;
        public float durationTime;
    }

    [Serializable]
    public class SIBonusInfo
    {
        public BonusType bonusType;
        public SICollectibleStatistics bonusStatistics;
        public UnityEvent OnBonusFinishEvent;
    }

    [Serializable]
    public class SIProjectileInfo
    {
        public WeaponType projectileType;
        public GameObject[] projectilePrefabs;
        public Transform[] projectileParents;
    }
}