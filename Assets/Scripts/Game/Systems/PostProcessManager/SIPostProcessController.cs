using System;
using System.Collections;
using Configs;
using Game.Systems;
using Sirenix.OdinInspector;
using SpaceInvaders;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Project.Systems {
    public class SIPostProcessController : MonoBehaviour {
        [SerializeField] PostProcessConfig _baseConfig;
        [SerializeField] PostProcessConfig _timeSpeedModificationConfig;
        [SerializeField] Volume _postProcessVolume;

        bool _isModifyingPosprocesses;
        PostProcessControllerState _postProcessState;
        Bloom _bloom;
        Vignette _vignette;
        Coroutine _applyPostprocessCoroutine;

        void Start() => Initialise();

        void OnDestroy() {
            if (_applyPostprocessCoroutine != null)
                StopCoroutine(_applyPostprocessCoroutine);
        }

        void Initialise() {
            _postProcessVolume.profile.TryGet(out _bloom);
            _postProcessVolume.profile.TryGet(out _vignette);
        }

        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();

        void SubscribeEvents() {
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
            SIGameplayEvents.OnWaveCoolDown += HandleOnWaveCooldown;
        }

        void UnsubscribeEvents() {
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
            SIGameplayEvents.OnWaveCoolDown -= HandleOnWaveCooldown;
        }

        void HandleOnBonusEnabled(BonusSettings settings) {
            if (settings.bonusType == BonusType.TimeModification) {
                TrySetTimeModificationPostprocessEffect();
            }
        }

        void HandleOnBonusDisabled(BonusSettings settings) {
            if (settings.bonusType == BonusType.TimeModification) {
                TrySetBasePostprocessEffect();
            }
        }

        void HandleOnWaveCooldown() {
            if(_applyPostprocessCoroutine != null)
                StopCoroutine(_applyPostprocessCoroutine);
            TrySetBasePostprocessEffect();
        }

        IEnumerator ApplyPostprocessCoroutine(float duration, 
            AnimationCurve curve, 
            Action<float> onPostprocessChange, PostProcessControllerState newState) {

            if (_postProcessState == newState) {
                yield break;
            }
            
            float progress;
            float time = 0.0f;
            _isModifyingPosprocesses = true;
            while (time < duration) {
                time += Time.deltaTime;
                progress = curve.Evaluate(time / duration);
                onPostprocessChange?.Invoke(progress);
                yield return WaitUtils.SkipFrames(1);
            }

            onPostprocessChange?.Invoke(1f);
            yield return WaitUtils.SkipFrames(1);
            _postProcessState = newState;
            _isModifyingPosprocesses = false;
        }

        void BaseToTimeModificationPostProcess(float progress) {
            _bloom.threshold.value =
                Mathf.Lerp(_baseConfig.bloomThreshold, _timeSpeedModificationConfig.bloomThreshold, progress);
            _bloom.intensity.value =
                Mathf.Lerp(_baseConfig.bloomIntensity, _timeSpeedModificationConfig.bloomIntensity, progress);
            _bloom.scatter.value =
                Mathf.Lerp(_baseConfig.bloomScatter, _timeSpeedModificationConfig.bloomScatter, progress);
            _bloom.tint.Interp(_baseConfig.bloomTintColor, _timeSpeedModificationConfig.bloomTintColor, progress);
            _vignette.intensity.value = Mathf.Lerp(_baseConfig.vignetteIntensity,
                _timeSpeedModificationConfig.vignetteIntensity, progress);
            _vignette.smoothness.value = Mathf.Lerp(_baseConfig.vignetteSmoothness,
                _timeSpeedModificationConfig.vignetteSmoothness, progress);
        }

        void TimeModificationToBasePostprocess(float progress) {
            _bloom.threshold.value =
                Mathf.Lerp(_timeSpeedModificationConfig.bloomThreshold, _baseConfig.bloomThreshold, progress);
            _bloom.intensity.value =
                Mathf.Lerp(_timeSpeedModificationConfig.bloomIntensity, _baseConfig.bloomIntensity, progress);
            _bloom.scatter.value =
                Mathf.Lerp(_timeSpeedModificationConfig.bloomScatter, _baseConfig.bloomScatter, progress);
            _bloom.tint.Interp(_timeSpeedModificationConfig.bloomTintColor, _baseConfig.bloomTintColor, progress);
            _vignette.intensity.value = Mathf.Lerp(_timeSpeedModificationConfig.vignetteIntensity,
                _baseConfig.vignetteIntensity, progress);
            _vignette.smoothness.value = Mathf.Lerp(_timeSpeedModificationConfig.vignetteSmoothness,
                _baseConfig.vignetteSmoothness, progress);
        }

        [Button]
        void TrySetTimeModificationPostprocessEffect() {
            _applyPostprocessCoroutine = StartCoroutine(
                ApplyPostprocessCoroutine(_timeSpeedModificationConfig.effectApplyDuration,
                    _timeSpeedModificationConfig.curve,
                    BaseToTimeModificationPostProcess,
                    PostProcessControllerState.TimeModificationPostprocess
                ));
        }
        
        [Button]
        void TrySetBasePostprocessEffect() {
            _applyPostprocessCoroutine = StartCoroutine(
                ApplyPostprocessCoroutine(_baseConfig.effectApplyDuration,
                    _baseConfig.curve,
                    TimeModificationToBasePostprocess,
                    PostProcessControllerState.BasicPostprocess
                ));
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
    }
}