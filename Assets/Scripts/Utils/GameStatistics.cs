
using PG.Game.Configs;

namespace PG.Game.Helpers {
    [System.Serializable]
    public class GameStatistics {
        int playerKills;
//        public int playerDeaths;

        SIPlayerStatistics _playerStats;

        public GameStatistics() {
            _playerStats = new SIPlayerStatistics();
        }

        public void UpdatePlayerKillsCounter() {
            ++_playerStats.playerKills;
        }

        public void UpdateEnemyBulletsDestroyedCounter() {
            ++_playerStats.bulletsDestroyed;
        }

        public void UpdateCurrentWaveCounter() {
            ++_playerStats.currentWave;
        }

        public void UpdatePlayerCollectedBonusesCounter() {
            ++_playerStats.bonusesCollected;
        }
    }
}