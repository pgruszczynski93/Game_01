using DG.Tweening;
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
        float _prevNoiseTresholdVal;
        float _currNoiseTresholdVal;
        float _prevEdgeWidthVal;
        float _currEdgeWidthVal;
        Material _material;

        public void Initialise()
        {
            _material = _renderer.material;
            _prevNoiseTresholdVal = 0;
            _prevEdgeWidthVal = 0;
        }

        public void SetDamageVFX(float damagePercent)
        {
            if (!_isColorTintActive)
                _isColorTintActive = true;

            _currNoiseTresholdVal = SIMathUtils.Remap(damagePercent, 0f, 1f, _minNoiseTreshold, _maxNoiseTreshold);
            _currEdgeWidthVal = SIMathUtils.Remap(damagePercent, 0f, 1f, _minEdgeWidth, _maxEdgeWidth);
            _material.SetInt(isColorTintActivePropId, 1);
            // _material.SetFloat(noiseTresholdPropId, _currNoiseTresholdVal);
            // _material.SetFloat(edgeWidthPropId, _currEdgeWidthVal);
            DOTween.To(() => _prevNoiseTresholdVal, (newVal) => _prevNoiseTresholdVal = newVal, _currNoiseTresholdVal,
                1.5f).OnUpdate(() =>
            {
                _material.SetFloat(noiseTresholdPropId, _prevNoiseTresholdVal);
            }).OnComplete(() =>
            {
                _prevNoiseTresholdVal = _currNoiseTresholdVal;
            });
            //     .SetLoops(-1, LoopType.Yoyo);
            DOTween.To(() => _prevEdgeWidthVal, (newVal) => _prevEdgeWidthVal = newVal, _currEdgeWidthVal, 1.5f)
                .OnUpdate(() =>
                {
                    _material.SetFloat(edgeWidthPropId, _prevEdgeWidthVal);
                }).OnComplete(() =>
                {
                    _prevEdgeWidthVal = _currEdgeWidthVal;

                });
            //     .SetLoops(-1, LoopType.Yoyo);
        }

        public void ResetDamageVFX()
        {
            _isColorTintActive = false;
            _prevNoiseTresholdVal = 0;
            _prevEdgeWidthVal = 0;
            _material.SetInt(isColorTintActivePropId, 0);
            _material.SetFloat(noiseTresholdPropId, 0f);
            _material.SetFloat(edgeWidthPropId, 0f);
        }
    }
}
