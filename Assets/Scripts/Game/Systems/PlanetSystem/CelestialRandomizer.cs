using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public abstract class CelestialRandomizer : MonoBehaviour {
        
        [SerializeField] protected Transform _parent;

        protected GameObject _celestial;
        protected MaterialPropertyBlock _matPropBlock;
        protected Renderer _renderer;

        public void Initialise() {
            _matPropBlock = new MaterialPropertyBlock();
        }

        [Button]
        public abstract void Randomize();
    }
}