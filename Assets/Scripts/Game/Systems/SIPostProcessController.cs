using System;
using System.Collections;
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

        bool _isModifyingPosprocesses;
        bool _isPostprocessModificationLocked;
        float _currentModifierValue;
        Bloom _bloom;
        Vignette _vignette;
        
        void Start() => Initialise();

        void Initialise() {
            _isModifyingPosprocesses = false;
            _currentModifierValue = 1f;
            _postProcessVolume.profile.TryGet(out _bloom);
            _postProcessVolume.profile.TryGet(out _vignette);
            RequestTimeSpeedModification();
        }
        
        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents() {
            SIGameplayEvents.OnWaveEnd += HandleOnWaveEnd;
            SIGameplayEvents.OnWaveStart += HandleOnWaveStart;
        }

        void UnsubscribeEvents() {
            SIGameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
            SIGameplayEvents.OnWaveStart -= HandleOnWaveStart;
        }

        void HandleOnWaveStart() {
            SetPostprocessModificationLock(false);
        }

        void HandleOnWaveEnd() {
            SetPostprocessModificationLock(true);
        }

        public void RequestTimeSpeedModification() {
            SIGameplayEvents.BroadcastOnSpeedModificationRequested(this);
        }

        //Note: This time this method will be used to interpolate between values during TimeSpeed changes (SiSpeedModificationManager coroutine works).
        public void SetTimeSpeedModifier(float modifier, float progress) {
            _isModifyingPosprocesses = Math.Abs(_currentModifierValue - modifier) > 1e-05f && progress < 1f;
            TryToLerpPostprocessValues(modifier, progress);
        }

        void SetPostprocessModificationLock(bool isEnabled) {
            _isPostprocessModificationLocked = isEnabled;
        }

        void TryToLerpPostprocessValues(float modifier, float progress) {

            if (_isPostprocessModificationLocked || !_isModifyingPosprocesses) {
                _currentModifierValue = modifier;
                return;
            }
            
            if (_currentModifierValue > modifier) {
                _bloom.threshold.value = Mathf.Lerp(_baseConfig.bloomThreshold, _speedModificationConfig.bloomThreshold, progress);
                _bloom.intensity.value = Mathf.Lerp(_baseConfig.bloomIntensity, _speedModificationConfig.bloomIntensity, progress);
                _bloom.scatter.value = Mathf.Lerp(_baseConfig.bloomScatter, _speedModificationConfig.bloomScatter, progress);
                _bloom.tint.Interp(_baseConfig.bloomTintColor, _speedModificationConfig.bloomTintColor, progress);
                _vignette.intensity.value = Mathf.Lerp(_baseConfig.vignetteIntensity, _speedModificationConfig.vignetteIntensity, progress);
                _vignette.smoothness.value = Mathf.Lerp(_baseConfig.vignetteSmoothness, _speedModificationConfig.vignetteSmoothness, progress);
            }
            else { 
                _bloom.threshold.value = Mathf.Lerp(_speedModificationConfig.bloomThreshold, _baseConfig.bloomThreshold, progress);
                _bloom.intensity.value = Mathf.Lerp(_speedModificationConfig.bloomIntensity, _baseConfig.bloomIntensity, progress);
                _bloom.scatter.value = Mathf.Lerp(_speedModificationConfig.bloomScatter, _baseConfig.bloomScatter, progress);
                _bloom.tint.Interp(_speedModificationConfig.bloomTintColor, _baseConfig.bloomTintColor, progress);
                _vignette.intensity.value = Mathf.Lerp(_speedModificationConfig.vignetteIntensity, _baseConfig.vignetteIntensity, progress);
                _vignette.smoothness.value = Mathf.Lerp(_speedModificationConfig.vignetteSmoothness, _baseConfig.vignetteSmoothness, progress);
            }
            
            _currentModifierValue = modifier;
        }

        IEnumerator PostProcessesLockRoutine() {
            // while()
            yield return null;
            //dorobić kod, który ogarnie dokonczenie postprocesu 
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
        
        // void OnEnable() => SubscribeEvents();
        // void OnDisable() => UnsubscribeEvents();
        //
        // void SubscribeEvents() {
        //     SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
        //     SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
        // }
        //
        // void UnsubscribeEvents() {
        //     SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
        //     SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
        // }
        // void HandleOnBonusEnabled(BonusSettings bonusSettings) {
        //     switch(bonusSettings.bonusType) {
        //         case BonusType.Health:
        //             break;
        //         case BonusType.Projectile:
        //             break;
        //         case BonusType.ShieldSystem:
        //             break;
        //         case BonusType.LaserBeam:
        //             break;
        //         case BonusType.RapidFire:
        //             break;
        //         case BonusType.TimeSlowDown:
        //             SetSpeedModificationPostprocessConfig();
        //             break;
        //     }
        // }
        //
        // void HandleOnBonusDisabled(BonusSettings bonusSettings) {
        //     switch(bonusSettings.bonusType) {
        //         case BonusType.Health:
        //             break;
        //         case BonusType.Projectile:
        //             break;
        //         case BonusType.ShieldSystem:
        //             break;
        //         case BonusType.LaserBeam:
        //             break;
        //         case BonusType.RapidFire:
        //             break;
        //         case BonusType.TimeSlowDown:
        //             SetBasePostProcessConfig();
        //             break;
        //     }
        // }
        //
    }
}
