using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class Planet : MonoBehaviour {
        const string PLANET_NAME = "Planet";
        const string RINGS_NAME = "Rings";
        
        static readonly int MainTex = Shader.PropertyToID("_MainTex");
        static readonly int RimColor = Shader.PropertyToID("_RimColor");
        static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        static readonly int BaseColorOpacity = Shader.PropertyToID("_BaseColorOpacity");

        [SerializeField] PlanetSettings _planetSettings;
        [SerializeField] Transform _planetSlot;
        [SerializeField] Renderer _planetRenderer;
        [SerializeField] GameObject _planetGameObject;
        MaterialPropertyBlock _matPropBlock;
        

        void Start() => Initialise();
        void Initialise() {
            _matPropBlock = new MaterialPropertyBlock();
        }
        
        [Button]
        public void RandomizePlanet() {
            GameObject[] planetVariants = _planetSettings.planetsGameObjects;
            GameObject[] rings = _planetSettings.ringsGameObjects;
            bool hasRings = Random.Range(0, 1) < _planetSettings.hasRingsTreshold;
            int maxPlanetsCount = planetVariants.Length;
            int maxRingsCount = rings.Length;
            int maxTextures = _planetSettings.availableTextures.Length;
            if (_planetGameObject == null) {
                _planetGameObject = Instantiate(planetVariants[Random.Range(0, maxPlanetsCount - 1)], _planetSlot);
                _planetGameObject.name = PLANET_NAME;                
            }
            int selectedPlanetTextureIndex = Random.Range(0, maxTextures - 1);
            float colorOpacity = Random.Range(0f, 1f);
            Color rimColor =  new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            Color planetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            _planetRenderer = _planetGameObject.GetComponent<Renderer>();
            _matPropBlock = new MaterialPropertyBlock();
            _planetRenderer.GetPropertyBlock(_matPropBlock);
            _matPropBlock.SetTexture(MainTex, _planetSettings.availableTextures[selectedPlanetTextureIndex]);
            _matPropBlock.SetColor(RimColor, rimColor);
            _matPropBlock.SetColor(BaseColor, planetColor);
            _matPropBlock.SetFloat(BaseColorOpacity, colorOpacity);
            _planetRenderer.SetPropertyBlock(_matPropBlock);
            if (hasRings) {
                GameObject newRigns = Instantiate(rings[Random.Range(0, maxRingsCount - 1)], _planetSlot);
                newRigns.name = "Rings";
            }
        }
    }
}