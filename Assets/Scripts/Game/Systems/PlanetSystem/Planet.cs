using Sirenix.OdinInspector;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class Planet : MonoBehaviour, IPoolable {

        [SerializeField] PlanetRandomizer _planetRandomizer;
        [SerializeField] RingsRandomizer _ringsRandomizer;
        [SerializeField] PlanetMovement _planetMovement;

        Transform _thisTransform;

        public Vector3 protoStartPos;
        public Vector3 protoFinishPos;
        
        void Start() => Initialise();
        void Initialise() {
            _planetRandomizer.Initialise();
            _ringsRandomizer.Initialise();
            _thisTransform = transform;
        }
       
        //PROTO
        public void UseObjectFromPool() {
            SetSpawnPosition(Vector3.back);
        }

        public void SetSpawnPosition(Vector3 spawnPos) {
            _thisTransform.position = protoStartPos;
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