using DG.Tweening;
using UnityEngine;

namespace SpaceInvaders {
    public class SIDamageVFX : MonoBehaviour {
        static readonly int fresnelColorPropId = Shader.PropertyToID("_FresnelColor");
        static readonly int fresnelPowerPropId = Shader.PropertyToID("_FresnelPower");
        static readonly int noiseScalePropId = Shader.PropertyToID("_NoiseScale");
        static readonly int noiseTresholdPropId = Shader.PropertyToID("_NoiseTreshold");
        static readonly int edgeWidthPropId = Shader.PropertyToID("_EdgeWidth");
        static readonly int isColorTintActivePropId = Shader.PropertyToID("_IsColorTintActive");

        [SerializeField] float _minNoiseTreshold;
        [SerializeField] float _maxNoiseTreshold;
        [SerializeField] float _minEdgeWidth;
        [SerializeField] float _maxEdgeWidth;
        [SerializeField] float _noiseChangeDuration;
        [SerializeField] float _edgeChangeDuration;
        [SerializeField] Renderer _renderer;

        bool _isColorTintActive;
        float _currNoiseTresholdVal;
        float _currEdgeWidthVal;
        Material _material;
        Tweener _noiseTweener;
        Tweener _edgeTweener;

        public void Initialise()
        {
            _material = _renderer.material;
            _noiseTweener = _material.DOFloat(0, noiseTresholdPropId, _noiseChangeDuration)
                .SetAutoKill(false)
                .Pause();
            _edgeTweener = _material.DOFloat(0, edgeWidthPropId, _edgeChangeDuration)
                .OnComplete(TryToEnableColorTint)
                .SetAutoKill(false)
                .Pause();
        }

        void TryToEnableColorTint()
        {
            if (_isColorTintActive)
                return;

            _isColorTintActive = true;
            _material.SetInt(isColorTintActivePropId, 1);
        }

        public void SetDamageVFX(float damagePercent)
        {
            _currNoiseTresholdVal = SIMathUtils.Remap(damagePercent, 0f, 1f, _minNoiseTreshold, _maxNoiseTreshold);
            _currEdgeWidthVal = SIMathUtils.Remap(damagePercent, 0f, 1f, _minEdgeWidth, _maxEdgeWidth);
            _noiseTweener.ChangeEndValue(_currNoiseTresholdVal, true).Restart();
            _edgeTweener.ChangeEndValue(_currEdgeWidthVal, true).Restart();
        }

        public void ResetDamageVFX()
        {
            _isColorTintActive = false;
            _currEdgeWidthVal = 0;
            _currNoiseTresholdVal = 0;
            _material.SetInt(isColorTintActivePropId, 0);
            _material.SetFloat(noiseTresholdPropId, 0f);
            _material.SetFloat(edgeWidthPropId, 0f);
        }
    }
}