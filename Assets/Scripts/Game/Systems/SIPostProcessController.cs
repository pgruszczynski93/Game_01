using System;
using Configs;
using Sirenix.OdinInspector;
using SpaceInvaders;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Project.Systems {
    public class SIPostProcessController : MonoBehaviour, IModifyTimeSpeedMultiplier {
        [SerializeField] PostProcessConfig _baseConfig;
        [SerializeField] PostProcessConfig _speedModificationConfig;
        [SerializeField] Volume _postProcessVolume;
         
        float _effectChangeDuration;
        Bloom _bloom;
        Vignette _vignette;
        
        void Start() => Initialise();
        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();

        void SubscribeEvents() {
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
        }

        void UnsubscribeEvents() {
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
        }

        void Initialise() {
            _postProcessVolume.profile.TryGet(out _bloom);
            _postProcessVolume.profile.TryGet(out _vignette);
        }
        
        public void SetTimeSpeedModifier(float modifier) {
            _effectChangeDuration = modifier;
        }
        
        void HandleOnBonusEnabled(BonusSettings bonusSettings) {
            switch(bonusSettings.bonusType) {
                case BonusType.Health:
                    break;
                case BonusType.Projectile:
                    break;
                case BonusType.ShieldSystem:
                    break;
                case BonusType.LaserBeam:
                    break;
                case BonusType.RapidFire:
                    break;
                case BonusType.TimeSlowDown:
                    SetSpeedModificationPostprocessConfig();
                    break;
            }
        }
        
        void HandleOnBonusDisabled(BonusSettings bonusSettings) {
            switch(bonusSettings.bonusType) {
                case BonusType.Health:
                    break;
                case BonusType.Projectile:
                    break;
                case BonusType.ShieldSystem:
                    break;
                case BonusType.LaserBeam:
                    break;
                case BonusType.RapidFire:
                    break;
                case BonusType.TimeSlowDown:
                    SetBasePostProcessConfig();
                    break;
            }
        }
        
        [Button]
        void SetBasePostProcessConfig() {
            _bloom.threshold.value = _baseConfig.bloomThreshold;
            _bloom.intensity.value = _baseConfig.bloomIntensity;
            _bloom.scatter.value = _baseConfig.bloomScatter;
            _bloom.tint.overrideState = false;
            _bloom.tint.value = _baseConfig.bloomTintColor;

            _vignette.intensity.value = _baseConfig.vignetteIntensity;
            _vignette.smoothness.value = _baseConfig.vignetteSmoothness;
        }

        [Button]
        void SetSpeedModificationPostprocessConfig() {
            _bloom.threshold.value = _speedModificationConfig.bloomThreshold;
            _bloom.intensity.value = _speedModificationConfig.bloomIntensity;
            _bloom.scatter.value = _speedModificationConfig.bloomScatter;
            _bloom.tint.overrideState = true;
            _bloom.tint.value = _speedModificationConfig.bloomTintColor;

            _vignette.intensity.value = _speedModificationConfig.vignetteIntensity;
            _vignette.smoothness.value = _speedModificationConfig.vignetteSmoothness;
        }
    }
}
