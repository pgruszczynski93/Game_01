using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "Asteroid Spawner Config", menuName = "Mindwalker Studio/Asteroid Spawner Config")]
    public class AsteroidSpawnerConfigurator : ScriptableObject
    {
        public AsteroidSpawnerSettings asteroidSpawnerSettings;
    }
}
