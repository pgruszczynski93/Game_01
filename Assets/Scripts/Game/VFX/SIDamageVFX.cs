using DG.Tweening;
using UnityEngine;

namespace SpaceInvaders {
    public class SIDamageVFX : MonoBehaviour {
        static readonly int noiseTresholdPropId = Shader.PropertyToID("_NoiseTreshold");
        static readonly int edgeWidthPropId = Shader.PropertyToID("_EdgeWidth");
        static readonly int isColorTintActivePropId = Shader.PropertyToID("_IsColorTintActive");
        static readonly int canClipAlpha = Shader.PropertyToID("_CanClipAlpha");

        [SerializeField] float _minNoiseTreshold;
        [SerializeField] float _maxNoiseTreshold;
        [SerializeField] float _minEdgeWidth;
        [SerializeField] float _maxEdgeWidth;
        [SerializeField] float _noiseChangeDuration;
        [SerializeField] float _edgeChangeDuration;
        [SerializeField] Renderer _renderer;

        bool _canClipAlpha;
        bool _isColorTintActive;
        float _currNoiseTresholdVal;
        float _currEdgeWidthVal;
        float _nextNoiseTresholdVal;
        float _nextEdgeWidthVal;
        MaterialPropertyBlock _matPropBlock;

        public void Initialise()
        {
            _matPropBlock = new MaterialPropertyBlock();
        }
        void TryToEnableBurnEffect()
        {
            if (!_canClipAlpha) {
                _canClipAlpha = true;
                UpdateSelectedFloatMaterialProperty(canClipAlpha, 1);
            }
            
            if (_isColorTintActive)
                return;

            _isColorTintActive = true;
            UpdateSelectedFloatMaterialProperty(isColorTintActivePropId, 1);
        }

        public void SetDamageVFX(float damagePercent)
        {
            TryToEnableBurnEffect();
            
            _nextNoiseTresholdVal = SIMathUtils.Remap(damagePercent, 0f, 1f, _minNoiseTreshold, _maxNoiseTreshold);
            _nextEdgeWidthVal = SIMathUtils.Remap(damagePercent, 0f, 1f, _minEdgeWidth, _maxEdgeWidth);
            
            DOTween.To(() => _currNoiseTresholdVal, newVal => _currNoiseTresholdVal = newVal, _nextNoiseTresholdVal, _noiseChangeDuration)
                .OnUpdate(() => UpdateSelectedFloatMaterialProperty(noiseTresholdPropId, _currNoiseTresholdVal));

            DOTween.To(() => _currEdgeWidthVal, newVal => _currEdgeWidthVal = newVal, _nextEdgeWidthVal, _edgeChangeDuration)
                .OnUpdate(() => UpdateSelectedFloatMaterialProperty(edgeWidthPropId, _currEdgeWidthVal));
        }

        void UpdateSelectedFloatMaterialProperty(int propertyId, float newValue) {
            _renderer.GetPropertyBlock(_matPropBlock);
            _matPropBlock.SetFloat(propertyId, newValue);
            _renderer.SetPropertyBlock(_matPropBlock);
        }
        
        public void ResetDamageVFX() {
            _canClipAlpha = false;
            _isColorTintActive = false;
            _currEdgeWidthVal = 0;
            _currNoiseTresholdVal = 0;
            UpdateSelectedFloatMaterialProperty(isColorTintActivePropId, 0);
            UpdateSelectedFloatMaterialProperty(canClipAlpha, 0);
            UpdateSelectedFloatMaterialProperty(noiseTresholdPropId, 0);
            UpdateSelectedFloatMaterialProperty(edgeWidthPropId, 0);
        }
    }
}