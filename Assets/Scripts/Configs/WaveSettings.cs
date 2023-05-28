using UnityEngine;

namespace PG.Game.Configs {
    [CreateAssetMenu(fileName = "Wave config", menuName = "Configs/Wave config")]
    public class WaveSettings : ScriptableObject {
        public WaveType waveType;
        public int enemiesInWave;
        public float waveStartCooldown;
        public float waveEndCoolDown;
        public float waveCoolDown;
    }

    public enum WaveType {
        Grid,
        //TO DO: WaveTypes - curves / boss / asteroids / ...
        // SPrawdzic jak wykonywane jest przeladowanie siatki - bo pewnie ma to wplyw na ustawianie strzelania 
        // zdebugować obecvna wersje
    }
}