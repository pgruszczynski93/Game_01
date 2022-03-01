using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class PlanetRandomizer : CelestialRandomizer {
        const string PLANET_NAME = "Planet";

        static readonly int MainTex = Shader.PropertyToID("_MainTex");
        static readonly int RimColor = Shader.PropertyToID("_RimColor");
        static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        static readonly int BaseColorOpacity = Shader.PropertyToID("_BaseColorOpacity");
        
        [SerializeField] PlanetSettings _planetSettings;

        [Button]
        public override void Randomize() {
            GameObject[] planetVariants = _planetSettings.planetsGameObjects;
            int maxPlanetsCount = planetVariants.Length;
            int maxTextures = _planetSettings.availableTextures.Length;
            if (_celestial == null) {
                _celestial = Instantiate(planetVariants[Random.Range(0, maxPlanetsCount - 1)], _parent);
                _celestial.name = PLANET_NAME;                
            }
            int selectedPlanetTextureIndex = Random.Range(0, maxTextures - 1);
            _renderer = _celestial.GetComponent<Renderer>();
            _matPropBlock = new MaterialPropertyBlock();
            _renderer.GetPropertyBlock(_matPropBlock);
            _matPropBlock.SetTexture(MainTex, _planetSettings.availableTextures[selectedPlanetTextureIndex]);
            _matPropBlock.SetColor(RimColor, SIMathUtils.GetRandomColorRGB());
            _matPropBlock.SetColor(BaseColor,  SIMathUtils.GetRandomColorRGB());
            _matPropBlock.SetFloat(BaseColorOpacity, Random.Range(0f, 1f));
            _renderer.SetPropertyBlock(_matPropBlock);
        }
    }
}