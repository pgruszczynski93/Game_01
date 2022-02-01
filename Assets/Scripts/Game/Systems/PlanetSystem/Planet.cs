using Sirenix.OdinInspector;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class Planet : MonoBehaviour, IPoolable {
        const string PLANET_NAME = "Planet";
        const string RINGS_NAME = "Rings";
        
        static readonly int MainTex = Shader.PropertyToID("_MainTex");
        static readonly int RimColor = Shader.PropertyToID("_RimColor");
        static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        static readonly int BaseColorOpacity = Shader.PropertyToID("_BaseColorOpacity");
        static readonly int RingsTintColor = Shader.PropertyToID("_TintColor");

        [SerializeField] PlanetSettings _planetSettings;
        [SerializeField] Transform _planetSlot;
        [SerializeField] Renderer _planetRenderer;
        [SerializeField] Renderer _ringsRenderer;
        [SerializeField] GameObject _planetGameObject;
        [SerializeField] GameObject _ringsGameObject;
        
        MaterialPropertyBlock _planetMatPropBlock;
        MaterialPropertyBlock _ringsMatPropBlock;

        void Start() => Initialise();
        void Initialise() {
            _planetMatPropBlock = new MaterialPropertyBlock();
            _ringsMatPropBlock = new MaterialPropertyBlock();
        }
        
        [Button]
        public void RandomizePlanet() {
            GameObject[] planetVariants = _planetSettings.planetsGameObjects;
            int maxPlanetsCount = planetVariants.Length;
            int maxTextures = _planetSettings.availableTextures.Length;
            if (_planetGameObject == null) {
                _planetGameObject = Instantiate(planetVariants[Random.Range(0, maxPlanetsCount - 1)], _planetSlot);
                _planetGameObject.name = PLANET_NAME;                
            }
            int selectedPlanetTextureIndex = Random.Range(0, maxTextures - 1);
            _planetRenderer = _planetGameObject.GetComponent<Renderer>();
            _planetMatPropBlock = new MaterialPropertyBlock();
            _planetRenderer.GetPropertyBlock(_planetMatPropBlock);
            _planetMatPropBlock.SetTexture(MainTex, _planetSettings.availableTextures[selectedPlanetTextureIndex]);
            _planetMatPropBlock.SetColor(RimColor, SIMathUtils.GetRandomColorRGB());
            _planetMatPropBlock.SetColor(BaseColor,  SIMathUtils.GetRandomColorRGB());
            _planetMatPropBlock.SetFloat(BaseColorOpacity, Random.Range(0f, 1f));
            _planetRenderer.SetPropertyBlock(_planetMatPropBlock);
            RandomizeRings();
        }

        [Button]
        void RandomizeRings() {
            GameObject[] rings = _planetSettings.ringsGameObjects;
            int maxRingsCount = rings.Length;

            if (_ringsGameObject == null) {
                _ringsGameObject = Instantiate(rings[Random.Range(0, maxRingsCount - 1)], _planetSlot);
                _ringsGameObject.name = RINGS_NAME;
            }
            
            bool areRingsEnabled = Random.Range(0, 2) == 1;
            float scaleFactor = Random.Range(_planetSettings.minRingsScale, _planetSettings.maxRingsScale);
            _ringsGameObject.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            _ringsGameObject.SetActive(areRingsEnabled);
            _ringsMatPropBlock = new MaterialPropertyBlock();
            _ringsRenderer = _ringsGameObject.GetComponent<Renderer>();
            _ringsRenderer.GetPropertyBlock(_ringsMatPropBlock);
            _ringsMatPropBlock.SetColor(RingsTintColor, SIMathUtils.GetRandomColorRGBA(true, 0.1f));
            _ringsRenderer.SetPropertyBlock(_ringsMatPropBlock);
        }

        public void UseObjectFromPool() {
            throw new System.NotImplementedException();
        }

        public void SetSpawnPosition(Vector3 spawnPos) {
            throw new System.NotImplementedException();
        }

        public void SetSpawnRotation(Vector3 spawnRot) {
            throw new System.NotImplementedException();
        }
    }
}