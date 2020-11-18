using UnityEngine.Rendering.HighDefinition;

namespace SpaceInvaders {
    [System.Serializable]
    public class SIGameStatistics {
        int playerKills;
//        public int playerDeaths;

        SIPlayerStatistics _playerStats;

        public SIGameStatistics()
        {
            _playerStats = new SIPlayerStatistics();
        }

        public void UpdatePlayerKillsCounter()
        {
            ++_playerStats.playerKills;
        }

        public void UpdateEnemyBulletsDestroyedCounter()
        {
            ++_playerStats.bulletsDestroyed;
        }

        public void UpdateCurrentWaveCounter()
        {
            ++_playerStats.currentWave;
        }

        public void UpdatePlayerCollectedBonusesCounter()
        {
            ++_playerStats.bonusesCollected;
        }
        
        
    }
}