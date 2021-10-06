using UnityEngine;

namespace Configs {
    [CreateAssetMenu(fileName = "Wave config", menuName = "Mindwalker Studio/Wave config")]
    public class WaveSettings : ScriptableObject {
        public WaveType waveType;
        public int enemiesInWave;
        public float newWaveCooldown;
        public float endWaveCooldown;
    }

    public enum WaveType {
        Grid,
        //TO DO: WaveTypes - curves / boss / asteroids / ...
        // SPrawdzic jak wykonywane jest przeladowanie siatki - bo pewnie ma to wplyw na ustawianie strzelania 
        // zdebugować obecvna wersje
    }
}