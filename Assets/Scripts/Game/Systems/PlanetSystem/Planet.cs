using Sirenix.OdinInspector;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class Planet : MonoBehaviour, IPoolable {
        [SerializeField] float _planetSizeMultiplier;
        [SerializeField] Transform _planetSlot;
        [SerializeField] GameObject _planetGraphicsParent;
        [SerializeField] PlanetRandomizer _planetRandomizer;
        [SerializeField] RingsRandomizer _ringsRandomizer;
        [SerializeField] PlanetMovement _planetMovement;

        bool _canRandomize;
        float _planetSizeY;
        Bounds _bounds;
        Transform _thisTransform;

        void Start() => Initialise();
        
        void Initialise() {
            _planetRandomizer.Initialise();
            _ringsRandomizer.Initialise();
            _thisTransform = transform;
            _planetGraphicsParent.SetActive(false);
            UpdatePlanetBoundsAndScale();
        }
        
#if UNITY_EDITOR
        void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            _bounds = _planetRandomizer.GetBounds();
            _bounds.Encapsulate(_ringsRandomizer.GetBounds());
            _planetSizeY = _bounds.size.y * _planetSizeMultiplier;
            Gizmos.DrawWireCube( _bounds.center, _bounds.size );
        }
#endif
        void UpdatePlanetBoundsAndScale() {
            float randomScaleMultiplier = Random.Range(1, _planetSizeMultiplier);
            transform.localScale = new Vector3(randomScaleMultiplier, randomScaleMultiplier, randomScaleMultiplier); 
            _bounds = _planetRandomizer.GetBounds();
            _bounds.Encapsulate(_ringsRandomizer.GetBounds());
            _planetSizeY = _bounds.size.y * _planetSizeMultiplier; //To make sure planet will hide completely during visibility check
        }

        public void SetSpawnPosition(Vector3 spawnPos) {
            _thisTransform.position = spawnPos + new Vector3(0, _bounds.extents.y, 0);
        }
        
        public void SetSpawnRotation(Vector3 spawnRot) {
            _thisTransform.eulerAngles = spawnRot;
        }
        
        public void PerformOnPoolActions() {
            if (!_planetGraphicsParent.activeInHierarchy)
                _planetGraphicsParent.SetActive(true);
            
            UpdatePlanetBoundsAndScale();
            _planetMovement.EnableMovement();
        }

        public void ManageScreenVisibility() {
            if (_thisTransform != null && SIScreenUtils.IsLowerThanVerticalScreenLimit(_thisTransform.position.y, -_planetSizeY))
                _planetMovement.DisableMovement();
        }

        public Bounds GetPlanetBounds() {
            return _bounds;
        }

        public bool IsMoving() {
            return _planetMovement.IsMoving;
        }

        [Button]
        public void RandomizePlanetAndRings() {
            RandomizePlanet();
            RandomizeRings();
            UpdatePlanetBoundsAndScale();
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