using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    [CreateAssetMenu(fileName = "Planets Collection", menuName = "Project/Planets Collection")]
    public class PlanetSettings : ScriptableObject {
        public Vector3 ringsScale;
        public GameObject[] planetsGameObjects;
        public GameObject[] ringsGameObjects;
        public Texture2D[] availableTextures;
    }
}