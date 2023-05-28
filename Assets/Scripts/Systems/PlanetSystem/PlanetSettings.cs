using UnityEngine;

namespace PG.Game.Systems.PlanetSystem {
    [CreateAssetMenu(fileName = "Planets Collection", menuName = "Configs/Planets Collection")]
    public class PlanetSettings : ScriptableObject {
        public float minRingsScale;
        public float maxRingsScale;
        public GameObject[] planetsGameObjects;
        public GameObject[] ringsGameObjects;
        public Texture2D[] availableTextures;
    }
}