using Sirenix.OdinInspector;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class Planet : MonoBehaviour, IPoolable {

        [SerializeField] PlanetRandomizer _planetRandomizer;
        [SerializeField] RingsRandomizer _ringsRandomizer;
        [SerializeField] PlanetMovement _planetMovement;
        

        void Start() => Initialise();
        void Initialise() {
            _planetRandomizer.Initialise();
            _ringsRandomizer.Initialise();
        }
        
       
        public void UseObjectFromPool() {
        }

        public void SetSpawnPosition(Vector3 spawnPos) {
        }

        public void SetSpawnRotation(Vector3 spawnRot) {
        }

        public void ManageScreenVisibility() {
        }

        [Button]
        public void RandomizePlanetAndRings() {
            RandomizePlanet();
            RandomizeRings();
        }

        [Button]
        void RandomizePlanet() {
            _planetRandomizer.Randomize();
        }

        [Button]
        void RandomizeRings() {
            _ringsRandomizer.Randomize();
        }
    }
}