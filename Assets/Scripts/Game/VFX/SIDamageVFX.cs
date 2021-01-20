﻿using DG.Tweening;
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
        float _nextNoiseTresholdVal;
        float _nextEdgeWidthVal;
        Material _material;
        MaterialPropertyBlock _matPropBlock;

        public void Initialise()
        {
            _material = _renderer.sharedMaterial;
            _matPropBlock = new MaterialPropertyBlock();
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
            _nextNoiseTresholdVal = SIMathUtils.Remap(damagePercent, 0f, 1f, _minNoiseTreshold, _maxNoiseTreshold);
            _nextEdgeWidthVal = SIMathUtils.Remap(damagePercent, 0f, 1f, _minEdgeWidth, _maxEdgeWidth);
            
            DOTween.To(() => _currNoiseTresholdVal, newVal => _currNoiseTresholdVal = newVal, _nextNoiseTresholdVal, _noiseChangeDuration)
                .OnUpdate(() => UpdateSelectedFloatMaterialProperty(noiseTresholdPropId, _currNoiseTresholdVal));

            DOTween.To(() => _currEdgeWidthVal, newVal => _currEdgeWidthVal = newVal, _nextEdgeWidthVal, _edgeChangeDuration)
                .OnUpdate(() => UpdateSelectedFloatMaterialProperty(edgeWidthPropId, _currEdgeWidthVal))
                .OnComplete(TryToEnableColorTint);
        }

        void UpdateSelectedFloatMaterialProperty(int propertyId, float newValue) {
            _renderer.GetPropertyBlock(_matPropBlock);
            _matPropBlock.SetFloat(propertyId, newValue);
            _renderer.SetPropertyBlock(_matPropBlock);
        }
        
        public void ResetDamageVFX()
        {
            _isColorTintActive = false;
            _currEdgeWidthVal = 0;
            _currNoiseTresholdVal = 0;
            UpdateSelectedFloatMaterialProperty(isColorTintActivePropId, 0);
            UpdateSelectedFloatMaterialProperty(noiseTresholdPropId, 0);
            UpdateSelectedFloatMaterialProperty(edgeWidthPropId, 0);
        }
    }
}