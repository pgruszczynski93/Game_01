namespace SpaceInvaders {
    [System.Serializable]
    public class SIGameStatistics {
        int playerKills;
//        public int playerDeaths;
        int enemyBulletsDestroyed;
        int currentWave;
        int playerCollectedBonuses;
        float playerScore;

        public void UpdatePlayerKillsCounter()
        {
            ++playerKills;
        }

        public void UpdateEnemyBulletsDestroyedCounter()
        {
            ++enemyBulletsDestroyed;
        }

        public void UpdateCurrentWaveCounter()
        {
            ++currentWave;
        }

        public void UpdatePlayerCollectedBonusesCounter()
        {
            ++playerCollectedBonuses;
        }
        
        
    }
}