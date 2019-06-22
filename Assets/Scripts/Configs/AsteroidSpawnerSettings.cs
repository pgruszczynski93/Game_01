using UnityEngine;

namespace SpaceInvaders
{
    [System.Serializable]
    public class AsteroidSpawnerSettings
    {
        [Range(0, 20)] public int asteroidVariantsCount;
        [Range(0, 100)] public int maxAsteroidsToSpawn;
        [Range(0f, 2.5f)] public float minTimeOffset;
        [Range(0f, 5f)] public float maxTimeOffset;
        public GameObject[] asteroidVariants;

    }
}