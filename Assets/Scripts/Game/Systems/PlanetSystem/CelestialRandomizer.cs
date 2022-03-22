using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public abstract class CelestialRandomizer : MonoBehaviour {
        
        [SerializeField] protected Transform _parent;

        protected GameObject _celestial;
        protected MaterialPropertyBlock _matPropBlock;
        protected Renderer _renderer;

        public void Initialise() {
            //Note: This line makes sure that we will get new planet on game start and proper refs assigned.
            Randomize();
        }

        [Button]
        public abstract void Randomize();
        
        public Bounds GetBounds() {
            return _renderer != null ? _renderer.bounds : new Bounds();
        }

        public GameObject GetGameObject() {
            return _celestial;
        }

        public bool IsObjectActiveInHierarchy() {
            return _celestial != null && _celestial.activeInHierarchy;
        }
    }
}