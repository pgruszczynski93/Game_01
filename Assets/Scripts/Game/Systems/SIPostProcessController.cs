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
        const float MODIFIER_TOLERANCE = 1e-05f;
        
        [SerializeField] float _defaultSpeedMultiplier;
        [SerializeField] float _timeSpeedModificationEffectApplyTreshold;
        [SerializeField] PostProcessConfig _baseConfig;
        [SerializeField] PostProcessConfig _timeSpeedModificationConfig;
        [SerializeField] Volume _postProcessVolume;
        
        bool _isModifyingPosprocesses;
        bool _isPostprocessModificationLocked;
        float _currentModifierValue;
        float _timeSpeedModificationProgress;
        Bloom _bloom;
        Vignette _vignette;
        Coroutine _waitForPostprocessLockRoutine;
        
        void Start() => Initialise();

        void OnDestroy() {
            if(_waitForPostprocessLockRoutine != null)
                StopCoroutine(_waitForPostprocessLockRoutine);
        }

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
            SIGameplayEvents.OnWaveCoolDown += HandleOnWaveCooldown;
            SIGameplayEvents.OnWaveStart += HandleOnWaveStart;
        }

        void UnsubscribeEvents() {
            SIGameplayEvents.OnWaveCoolDown -= HandleOnWaveCooldown;
            SIGameplayEvents.OnWaveStart -= HandleOnWaveStart;
        }

        void HandleOnWaveStart() {
            SetPostprocessModificationLock(false);
        }

        void HandleOnWaveCooldown() {
            _waitForPostprocessLockRoutine = StartCoroutine(PostProcessesLockRoutine());
        }

        public void RequestTimeSpeedModification() {
            SIGameplayEvents.BroadcastOnSpeedModificationRequested(this);
        }

        //Note: This time this method will be used to interpolate between values during TimeSpeed changes (SiSpeedModificationManager coroutine works).
        public void SetTimeSpeedModifier(float modifier, float progress) {
            _isModifyingPosprocesses = Math.Abs(_currentModifierValue - modifier) > 1e-05f && progress < 1f;
            TryMakeTimeModificationEffect(modifier, progress);
        }

        bool CanUseTimeModificationEffect() {
            return SIPlayerBonusesManager.IsBonusActive(BonusType.TimeModification);
            // && _timeSpeedModificationProgress < 
            
            //todo: obsluzyc przypadek uzycia efektu do zmiany szybkosci symulacji czasu 
            // kiedy mam aktywny energyboost i timespeed
        }

        bool CanRestoreNormalTimeEffect() {
            return SIPlayerBonusesManager.IsBonusActive(BonusType.TimeModification);
            // &&
        }

        void SetPostprocessModificationLock(bool isEnabled) {
            _isPostprocessModificationLocked = isEnabled;
        }

        void TryMakeTimeModificationEffect(float modifier, float progress) {
            if (_isPostprocessModificationLocked || !_isModifyingPosprocesses) {
                _currentModifierValue = modifier;
                return;
            }
            
            if (_currentModifierValue > modifier) {
                _bloom.threshold.value = Mathf.Lerp(_baseConfig.bloomThreshold, _timeSpeedModificationConfig.bloomThreshold, progress);
                _bloom.intensity.value = Mathf.Lerp(_baseConfig.bloomIntensity, _timeSpeedModificationConfig.bloomIntensity, progress);
                _bloom.scatter.value = Mathf.Lerp(_baseConfig.bloomScatter, _timeSpeedModificationConfig.bloomScatter, progress);
                _bloom.tint.Interp(_baseConfig.bloomTintColor, _timeSpeedModificationConfig.bloomTintColor, progress);
                _vignette.intensity.value = Mathf.Lerp(_baseConfig.vignetteIntensity, _timeSpeedModificationConfig.vignetteIntensity, progress);
                _vignette.smoothness.value = Mathf.Lerp(_baseConfig.vignetteSmoothness, _timeSpeedModificationConfig.vignetteSmoothness, progress);
                _timeSpeedModificationProgress = 1 - progress;
            }
            else if (_currentModifierValue < modifier){ 
                _bloom.threshold.value = Mathf.Lerp(_timeSpeedModificationConfig.bloomThreshold, _baseConfig.bloomThreshold, progress);
                _bloom.intensity.value = Mathf.Lerp(_timeSpeedModificationConfig.bloomIntensity, _baseConfig.bloomIntensity, progress);
                _bloom.scatter.value = Mathf.Lerp(_timeSpeedModificationConfig.bloomScatter, _baseConfig.bloomScatter, progress);
                _bloom.tint.Interp(_timeSpeedModificationConfig.bloomTintColor, _baseConfig.bloomTintColor, progress);
                _vignette.intensity.value = Mathf.Lerp(_timeSpeedModificationConfig.vignetteIntensity, _baseConfig.vignetteIntensity, progress);
                _vignette.smoothness.value = Mathf.Lerp(_timeSpeedModificationConfig.vignetteSmoothness, _baseConfig.vignetteSmoothness, progress);
                _timeSpeedModificationProgress = progress;
            }
            
            _currentModifierValue = modifier;
        }

        IEnumerator PostProcessesLockRoutine() {
            while (Math.Abs(_currentModifierValue - _defaultSpeedMultiplier) > MODIFIER_TOLERANCE) {
                yield return WaitUtils.SkipFrames(1);
            }
            SetPostprocessModificationLock(true);
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
            _bloom.threshold.value = _timeSpeedModificationConfig.bloomThreshold;
            _bloom.intensity.value = _timeSpeedModificationConfig.bloomIntensity;
            _bloom.scatter.value = _timeSpeedModificationConfig.bloomScatter;
            _bloom.tint.overrideState = true;
            _bloom.tint.value = _timeSpeedModificationConfig.bloomTintColor;

            _vignette.intensity.value = _timeSpeedModificationConfig.vignetteIntensity;
            _vignette.smoothness.value = _timeSpeedModificationConfig.vignetteSmoothness;
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
