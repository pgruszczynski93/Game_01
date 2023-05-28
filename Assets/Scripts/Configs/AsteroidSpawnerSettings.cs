using PG.Game.Asteroids;
using UnityEngine;

namespace PG.Game.Configs {
    [System.Serializable]
    public class AsteroidSpawnerSettings {
        [Range(0, 20)] public int asteroidVariantsCount;
        [Range(0, 100)] public int maxAsteroidsToSpawn;
        public AsteroidBehaviour[] asteroidVariants;
    }
}