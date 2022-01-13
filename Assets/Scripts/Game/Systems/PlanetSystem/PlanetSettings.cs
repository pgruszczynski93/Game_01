using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    [CreateAssetMenu(fileName = "Planets Collection", menuName = "Project/Planets Collection")]
    public class PlanetSettings : ScriptableObject {
        [Range(0, 1)] public float hasRingsTreshold;
        public GameObject[] planetsGameObjects;
        public GameObject[] ringsGameObjects;
    }
}