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

        Bounds _bounds;

        void Start() => Initialise();
        void Initialise() {
            _planetRandomizer.Initialise();
            _ringsRandomizer.Initialise();
            _thisTransform = transform;
            UpdatePlanetBounds();
        }
        
        //todo:
        //1 spawnowanie planety wzgledem glebokosci boxa 
        // przeladowywanie pooli na jakims ewencie.?
        // proto - 
        // spawnowanie planety wewnatrz boxa do obszaru planet - pod jakims buttnem
        
        #if UNITY_EDITOR
        void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            UpdatePlanetBounds();
            Gizmos.DrawWireCube( _bounds.center, _bounds.size );
        }
        #endif

        void UpdatePlanetBounds() {
            _bounds = _planetRandomizer.GetBounds();
            _bounds.Encapsulate(_ringsRandomizer.GetBounds());
        }
        public void UseObjectFromPool() {
        }

        public void SetSpawnPosition(Vector3 spawnPos) {
            //ustawić planete tak by była odpowiednio daleko od gracza - najlepiej wzgledem jakiegoś prostopadloscianu
            // _thisTransform.position = spawnPos;
            var newX = spawnPos
            transform.position = spawnPos + new Vector3(0, _bounds.extents.y, 0);
        }

        public void SetSpawnRotation(Vector3 spawnRot) {
            //todo - zrobić clampowanie kąta obrotu tak by wygladało dobrze
            // _thisTransform.position = spawnPos;
            transform.eulerAngles = spawnRot;
        }

        public void ManageScreenVisibility() {
            if (_thisTransform && SIScreenUtils.IsInVerticalWorldScreenLimit(_thisTransform.position.y))
                return;

            // Update();
            // zatrzymaj i zresetuj rzeczy
        }

        public Bounds GetPlanetBounds() {
            //If doesn't work: UpdateBounds();
            return _bounds;
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