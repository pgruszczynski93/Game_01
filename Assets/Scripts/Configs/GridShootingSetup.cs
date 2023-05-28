using UnityEngine;

namespace PG.Game.Configs {
    [CreateAssetMenu(fileName = "Grid shooting config", menuName = "Configs/Grid Shooting")]
    public class GridShootingSetup : ScriptableObject {
        public GridShootingSettings shootingSettings;
    }
}