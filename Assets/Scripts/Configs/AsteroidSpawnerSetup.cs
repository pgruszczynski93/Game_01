using UnityEngine;

namespace PG.Game.Configs {
    [CreateAssetMenu(fileName = "Asteroid Spawner Config", menuName = "Configs/Asteroid Spawner")]
    public class AsteroidSpawnerSetup : ScriptableObject {
        public AsteroidSpawnerSettings asteroidSpawnerSettings;
    }
}