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
        Bounds _planetBounds;

        void Start() => Initialise();
        void Initialise() {
            _planetRandomizer.Initialise();
            _ringsRandomizer.Initialise();
            _thisTransform = transform;
            _planetBounds.Encapsulate(_planetRandomizer.GetBounds());
            _planetBounds.Encapsulate(_ringsRandomizer.GetBounds());
        }
        
        //todo:
        //1 spawnowanie planety wzgledem glebokosci boxa 
        // dorobienie boundsow
        // przeladowywanie pooli na jakims ewencie.?
        // void OnDrawGizmosSelected() {
        //     Gizmos.color = Color.blue;
        //     _planetBounds.Encapsulate(_planetRandomizer.GetBounds());
        //     _planetBounds.Encapsulate(_ringsRandomizer.GetBounds());
        //     var size = new Vector3(_planetBounds.size.x * _planetSlot.localScale.x,
        //         _planetBounds.size.y * _planetSlot.localScale.y,
        //         _planetBounds.size.z * _planetSlot.localScale.z);
        //     Gizmos.DrawWireCube(_planetBounds.center, size);
        // }

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