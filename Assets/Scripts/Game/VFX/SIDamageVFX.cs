using DG.Tweening;
using ScriptableSettings;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class SIDamageVFX : MonoBehaviour {
        static readonly int noiseTresholdPropId = Shader.PropertyToID("_NoiseTreshold");
        static readonly int edgeWidthPropId = Shader.PropertyToID("_EdgeWidth");
        static readonly int isColorTintActivePropId = Shader.PropertyToID("_IsColorTintActive");
        static readonly int canClipAlpha = Shader.PropertyToID("_CanClipAlpha");

        [SerializeField] bool _hasParticles;
        [SerializeField] DamageVfxSettings _damageEffectSettings;
        [SerializeField] Renderer _renderer;
        [ShowIf("_hasParticles"),SerializeField] ParticleSystem _fireParticleSystem;
        [ShowIf("_hasParticles"),SerializeField] ParticleSystem _sparksParticleSystem;

        bool _canClipAlpha;
        bool _isColorTintActive;
        bool _particlesEnabled;

        float _currNoiseTresholdVal;
        float _currEdgeWidthVal;
        float _nextNoiseTresholdVal;
        float _nextEdgeWidthVal;
        
        float _currentParticlesShapeAxisScaleValue;
        float _nextParticlesShapeAxisScaleValue;
        MaterialPropertyBlock _matPropBlock;
        
        ParticleSystem.MainModule _fireParticlesMainModule;
        ParticleSystem.ShapeModule _fireParticlesShapeModule;

        public void Initialise()
        {
            _matPropBlock = new MaterialPropertyBlock();
            _fireParticlesMainModule = _fireParticleSystem.main;
            _fireParticlesShapeModule = _fireParticleSystem.shape;
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

        void TryEnableParticles() {
            if (!_hasParticles || _particlesEnabled)
                return;

            _particlesEnabled = true;
            PlayParticlesSystems();
        }
        
        public void SetDamageVFX(float damagePercent)
        {
            TryToEnableBurnEffect();
            ManageShaderSettings(damagePercent);
            TryEnableParticles();
            ManageParticlesSettings(damagePercent);
        }
        
        void ManageShaderSettings(float damagePercent) {
            var shaderSetup = _damageEffectSettings.shaderSetup;
            _nextNoiseTresholdVal =
                SIMathUtils.Remap(damagePercent, 0f, 1f, shaderSetup.minNoiseTreshold, shaderSetup.maxNoiseTreshold);
            _nextEdgeWidthVal = SIMathUtils.Remap(damagePercent, 0f, 1f, shaderSetup.minEdgeWidth, shaderSetup.maxEdgeWidth);

            DOTween.To(() => _currNoiseTresholdVal, newVal => _currNoiseTresholdVal = newVal, _nextNoiseTresholdVal,
                    shaderSetup.noiseChangeDuration)
                .OnUpdate(() => UpdateSelectedFloatMaterialProperty(noiseTresholdPropId, _currNoiseTresholdVal));

            DOTween.To(() => _currEdgeWidthVal, newVal => _currEdgeWidthVal = newVal, _nextEdgeWidthVal,
                    shaderSetup.edgeChangeDuration)
                .OnUpdate(() => UpdateSelectedFloatMaterialProperty(edgeWidthPropId, _currEdgeWidthVal));
        }

        void ManageParticlesSettings(float damagePercent) {
            if (!_hasParticles)
                return;

            var particlesSetup = _damageEffectSettings.particleSetup;
            _fireParticlesMainModule.maxParticles = (int) SIMathUtils.Remap(damagePercent, 0f, 1f, particlesSetup.minParticlesCount, particlesSetup.maxParticlesCount);
            _nextParticlesShapeAxisScaleValue = SIMathUtils.Remap(damagePercent, 0f, 1f, particlesSetup.minScaleAxisValue, particlesSetup.maxScaleAxisValue);
            
            DOTween.To(() => _currentParticlesShapeAxisScaleValue, newVal => _currentParticlesShapeAxisScaleValue = newVal, _nextParticlesShapeAxisScaleValue,
                    particlesSetup.scaleChangeDuration)
                .OnUpdate(UpdateParticlesShapeModule);
        }

        void UpdateSelectedFloatMaterialProperty(int propertyId, float newValue) {
            _renderer.GetPropertyBlock(_matPropBlock);
            _matPropBlock.SetFloat(propertyId, newValue);
            _renderer.SetPropertyBlock(_matPropBlock);
        }

        void UpdateParticlesShapeModule() {
            _fireParticlesShapeModule.scale =
                new Vector3(_nextParticlesShapeAxisScaleValue, _nextParticlesShapeAxisScaleValue, 1);
        }
        
        void PlayParticlesSystems() {
            _fireParticleSystem.Play();
            _sparksParticleSystem.Play();
        }

        void StopParticlesSystems() {
            _fireParticleSystem.Stop();
            _sparksParticleSystem.Stop();
        }
        
        public void ResetDamageVFX() {
            _canClipAlpha = false;
            _isColorTintActive = false;
            _particlesEnabled = false;
            _currEdgeWidthVal = 0;
            _currNoiseTresholdVal = 0;
            _fireParticlesMainModule.maxParticles = 0;
            _fireParticlesShapeModule.scale = Vector3.zero;
            StopParticlesSystems();
            UpdateSelectedFloatMaterialProperty(isColorTintActivePropId, 0);
            UpdateSelectedFloatMaterialProperty(canClipAlpha, 0);
            UpdateSelectedFloatMaterialProperty(noiseTresholdPropId, 0);
            UpdateSelectedFloatMaterialProperty(edgeWidthPropId, 0);
        }
    }
}