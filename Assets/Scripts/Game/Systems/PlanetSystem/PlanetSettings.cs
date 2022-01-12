using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    [CreateAssetMenu(fileName = "Planets Collection", menuName = "Project/Planets Collection")]
    public class PlanetSettings : ScriptableObject {
        public GameObject[] planetsGameObjects;
        public GameObject[] ringsGameObjects;
    }
}