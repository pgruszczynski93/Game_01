using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class Planet : MonoBehaviour {
        const string PLANET_NAME = "Planet";
        const string RINGS_NAME = "Rings";
        
        [SerializeField] PlanetSettings _planetSettings;
        [SerializeField] Transform _planetSlot;
        
        [Button]
        public void RandomizePlanet() {
            GameObject[] planetVariants = _planetSettings.planetsGameObjects;
            GameObject[] rings = _planetSettings.ringsGameObjects;
            bool hasRings = Random.Range(0, 1) < _planetSettings.hasRingsTreshold;
            int maxPlanetsCount = planetVariants.Length;
            int maxRingsCount = rings.Length;
            GameObject newPlanet = Instantiate(planetVariants[Random.Range(0, maxPlanetsCount - 1)], _planetSlot);
            newPlanet.name = PLANET_NAME;
            if (hasRings) {
                GameObject newRigns = Instantiate(rings[Random.Range(0, maxRingsCount - 1)], _planetSlot);
                newRigns.name = "Rings";
            }
        }
    }
}