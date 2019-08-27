using UnityEngine;

namespace SpaceInvaders
{
    [System.Serializable]
    public class AsteroidSpawnerSettings
    {
        [Range(0, 20)] public int asteroidVariantsCount;
        [Range(0, 100)] public int maxAsteroidsToSpawn;
        public GameObject[] asteroidVariants;

    }
}