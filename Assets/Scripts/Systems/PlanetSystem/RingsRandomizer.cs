using Sirenix.OdinInspector;
using UnityEngine;

namespace PG.Game.Systems.PlanetSystem {
    public class RingsRandomizer : CelestialRandomizer {
        const string RINGS_NAME = "Rings";

        static readonly int RingsTintColor = Shader.PropertyToID("_TintColor");

        [SerializeField] PlanetSettings _planetSettings;


        [Button]
        public override void Randomize() {
            GameObject[] rings = _planetSettings.ringsGameObjects;
            int maxRingsCount = rings.Length;

            if (_celestial == null) {
                _celestial = Instantiate(rings[Random.Range(0, maxRingsCount - 1)], _parent);
                _celestial.name = RINGS_NAME;
            }

            bool areRingsEnabled = Random.Range(0, 2) == 1;
            float scaleFactor = Random.Range(_planetSettings.minRingsScale, _planetSettings.maxRingsScale);
            _celestial.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            _celestial.SetActive(areRingsEnabled);
            _matPropBlock = new MaterialPropertyBlock();
            if (_renderer == null)
                _renderer = _celestial.GetComponent<Renderer>();
            _renderer.GetPropertyBlock(_matPropBlock);
            _matPropBlock.SetColor(RingsTintColor, MathUtils.GetRandomColorRGBA(true, 0.1f));
            _renderer.SetPropertyBlock(_matPropBlock);
        }
    }
}