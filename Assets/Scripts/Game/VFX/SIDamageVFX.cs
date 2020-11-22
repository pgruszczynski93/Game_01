using UnityEngine;

namespace SpaceInvaders {
    public class SIDamageVFX : MonoBehaviour {

        const string FRESNEL_COLOR = "_FresnelColor";
        const string FRESNEL_POWER = "_FresnelPower";
        const string NOISE_SCALE = "_NoiseScale";
        const string NOISE_TRESHOLD = "_NoiseTreshold";
        const string EDGE_WIDTH = "_EdgeWidth";
        const string IS_COLOR_TINT_ACTIVE = "_IsColorTintActive";
        
        static readonly int fresnelColorPropId = Shader.PropertyToID(FRESNEL_COLOR);
        static readonly int fresnelPowerPropId = Shader.PropertyToID(FRESNEL_POWER);
        static readonly int noiseScalePropId = Shader.PropertyToID(NOISE_SCALE);
        static readonly int noiseTresholdPropId = Shader.PropertyToID(NOISE_TRESHOLD);
        static readonly int edgeWidthPropId = Shader.PropertyToID(EDGE_WIDTH);
        static readonly int isColorTintActivePropId = Shader.PropertyToID(IS_COLOR_TINT_ACTIVE);

        [SerializeField] float _minNoiseTreshold;
        [SerializeField] float _maxNoiseTreshold;
        [SerializeField] float _minEdgeWidth;
        [SerializeField] float _maxEdgeWidth;
        [SerializeField] Renderer _renderer;

        bool _isColorTintActive;
        Material _material;

        public void Initialise()
        {
            _material = _renderer.material;
        }

        public void SetDamageVFX(float damagePercent)
        {
            if (!_isColorTintActive)
                _isColorTintActive = true;
            
            _material.SetInt(isColorTintActivePropId, 1);
            _material.SetFloat(noiseTresholdPropId, SIMathUtils.Remap(damagePercent, 0f, 1f, _minNoiseTreshold, _maxNoiseTreshold));
            _material.SetFloat(edgeWidthPropId, SIMathUtils.Remap(damagePercent,0f, 1f, _minEdgeWidth, _maxEdgeWidth));
        }

        public void ResetDamageVFX()
        {
            _isColorTintActive = false;
            _material.SetInt(isColorTintActivePropId, 0);
            _material.SetFloat(noiseTresholdPropId, 0f);
            _material.SetFloat(edgeWidthPropId, 0f);
        }
    }
}
