using Sirenix.OdinInspector;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class Planet : MonoBehaviour, IPoolable {

        [SerializeField] Transform _planetSlot;
        [SerializeField] PlanetRandomizer _planetRandomizer;
        [SerializeField] RingsRandomizer _ringsRandomizer;
        [SerializeField] PlanetMovement _planetMovement;

        Transform _thisTransform;

        void Start() => Initialise();
        void Initialise() {
            _planetRandomizer.Initialise();
            _ringsRandomizer.Initialise();
            _thisTransform = transform;
        }
        
        //todo:
        //1 spawnowanie planety wzgledem glebokosci boxa 
        // przeladowywanie pooli na jakims ewencie.?
        // proto - 
        // spawnowanie planety wewnatrz boxa do obszaru planet - pod jakims buttnem
        void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            Bounds planetBounds = _planetRandomizer.GetBounds();
            if (_planetRandomizer.IsObjectActiveInHierarchy()) 
                Gizmos.DrawWireCube( planetBounds.center, planetBounds.size );
            Bounds ringBounds = _ringsRandomizer.GetBounds();
            if (_ringsRandomizer.IsObjectActiveInHierarchy()) 
                Gizmos.DrawWireCube( ringBounds.center, ringBounds.size );
        }

        public void UseObjectFromPool() {
        }

        public void SetSpawnPosition(Vector3 spawnPos) {
            //ustawić planete tak by była odpowiednio daleko od gracza - najlepiej wzgledem jakiegoś prostopadloscianu
            _thisTransform.position = spawnPos;
        }

        public void SetSpawnRotation(Vector3 spawnRot) {
            //todo - zrobić clampowanie kąta obrotu tak by wygladało dobrze
            _thisTransform.eulerAngles = spawnRot;
        }

        public void ManageScreenVisibility() {
            if (_thisTransform && SIScreenUtils.IsInVerticalWorldScreenLimit(_thisTransform.position.y))
                return;

            // Update();
            // zatrzymaj i zresetuj rzeczy
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