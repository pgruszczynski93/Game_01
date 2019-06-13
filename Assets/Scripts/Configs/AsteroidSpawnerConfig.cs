using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "Asteroid Spawner Config", menuName = "Scriptable Configs/Asteroid Spawner Config")]
    public class AsteroidSpawnerConfig : ScriptableObject
    {
        public int asteroidVariantsCount;
        public int maxAsteroidsToSpawn;
        public float minTimeOffset;
        public float maxTimeOffset;
    }
}