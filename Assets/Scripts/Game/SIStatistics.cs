using System;

namespace SpaceInvaders
{
    [Serializable]
    public class SIStatistics
    {
        public bool isAlive;
        public bool canDropBonus;
        public int currentHealth;
        public int currentScore;
        public int currentWave;
    }

}