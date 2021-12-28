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
        [SerializeField] AnimationCurve timeModificationInCurve;
        [SerializeField] AnimationCurve timeModificationOutCurve;
        [SerializeField] PostProcessConfig _baseConfig;
        [SerializeField] PostProcessConfig _timeSpeedModificationConfig;
        [SerializeField] PostProcessConfig _energyBoostWithTimeModificationConfig;
        [SerializeField] Volume _postProcessVolume;

        bool _isModifyingPosprocesses;
        bool _isEnergyBoostActive;
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
            SIEventsHandler.OnIndependentUpdate += HandleOnIndependentUpdate;
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
            SIGameplayEvents.OnWaveCoolDown += HandleOnWaveCooldown;
        }

        void UnsubscribeEvents() {
            SIEventsHandler.OnIndependentUpdate -= HandleOnIndependentUpdate;
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
            SIGameplayEvents.OnWaveCoolDown -= HandleOnWaveCooldown;
        }

        void HandleOnIndependentUpdate() {
            //TODO: REMOVE INDEPENDENT UPDATE & FIX TINT
            TryToggleTimeModificationWithEnergyBoostPostprocess();
        }

        void HandleOnBonusEnabled(BonusSettings settings) {
            if (settings.bonusType == BonusType.TimeModification) {
                TrySetTimeModificationPostprocessEffect();
            }
        }

        void HandleOnBonusDisabled(BonusSettings settings) {
            switch (settings.bonusType) {
                case BonusType.TimeModification:
                    TrySetBasePostprocessEffect();
                    break;
                case BonusType.EnergyBoost:
                    EnableEnergyBoostAndManagePostprocess(false);
                    break;
            }
        }

        void HandleOnWaveCooldown() {
            if(_applyPostprocessCoroutine != null)
                StopCoroutine(_applyPostprocessCoroutine);
            TrySetBasePostprocessEffect();
        }

        void TryToggleTimeModificationWithEnergyBoostPostprocess() {
            if (_isEnergyBoostActive || !SIPlayerBonusesManager.IsBonusActive(BonusType.EnergyBoost)) 
                return;
            if (!SIPlayerBonusesManager.IsBonusActive(BonusType.TimeModification))
                return;
            
            _isEnergyBoostActive = true;
            EnableEnergyBoostAndManagePostprocess(true);
        }

        void EnableEnergyBoostAndManagePostprocess(bool isEnabled) {
            _isEnergyBoostActive = isEnabled;
            if (_isEnergyBoostActive) {
                SetBloomForTimeModificationWithBoost();
            }
            else {
                TryToRestoreBaseBloomTint();
            }
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
        void TrySetBasePostprocessEffect() {
            _applyPostprocessCoroutine = StartCoroutine(
                ApplyPostprocessCoroutine(_baseConfig.effectApplyDuration,
                    timeModificationOutCurve,
                    TimeModificationToBasePostprocess,
                    PostProcessControllerState.BasicPostprocess
                ));
        }
        
        [Button]
        void TrySetTimeModificationPostprocessEffect() {
            _applyPostprocessCoroutine = StartCoroutine(
                ApplyPostprocessCoroutine(_timeSpeedModificationConfig.effectApplyDuration,
                    timeModificationInCurve,
                    BaseToTimeModificationPostProcess,
                    PostProcessControllerState.TimeModificationPostprocess
                ));
        }

        [Button]
        void SetBloomForTimeModificationWithBoost() {
            _bloom.tint.Override(_energyBoostWithTimeModificationConfig.bloomTintColor);
        }

        [Button]
        void TryToRestoreBaseBloomTint() {
            _bloom.tint.Override(SIPlayerBonusesManager.IsBonusActive(BonusType.TimeModification)
                ? _timeSpeedModificationConfig.bloomTintColor
                : _baseConfig.bloomTintColor);
        }
    }
}