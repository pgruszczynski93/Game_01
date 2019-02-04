using System.Collections.Generic;
using System;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public class SIStatistics
    {
        public int currentHealth;
        public int currentScore;
        public int currentWave;
        public WeaponType currentWeaponType;
        //public List<BonusType> activeBonuses;
    }

    [Serializable]
    public class SICollectibleStatistics
    {
        public int gainedHealth;
        public int gainedScore;
        public WeaponType gainedWeaponType;
    }

    [Serializable]
    public class SIBonusInfo
    {
        public BonusType bonusType;
        public SICollectibleStatistics bonusStatistics;
    }

    [Serializable]
    public class SIProjectileInfo
    {
        public WeaponType projectileType;
        public GameObject[] projectilePrefabs;
        public Transform[] projectileParents;
    }
}