using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "Scriptable Settings Master", menuName = "Mindwalker Studio/Configurator Master")]
    public class ScriptableSettingsMaster : ScriptableObject
    {
        public AsteroidSpawnerConfigurator asteroidSpawnerConfigurator;
    }
}