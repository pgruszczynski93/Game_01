using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    [CreateAssetMenu(fileName = "Planets Collection", menuName = "Project/Planets Collection")]
    public class PlanetSettings : ScriptableObject {
        public float minRingsScale;
        public float maxRingsScale;
        public GameObject[] planetsGameObjects;
        public GameObject[] ringsGameObjects;
        public Texture2D[] availableTextures;
    }
}