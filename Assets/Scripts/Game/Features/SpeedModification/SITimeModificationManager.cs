using System;
using System.Collections;
using System.Collections.Generic;
using Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class SITimeModificationManager : SIBonusDrivenBehaviour {
        
        public static bool IsModifyingSpeed;
        
        [ShowInInspector] float _currentSpeedModifier;
        [ShowInInspector] float _timeSpeedModificationProgress;
        
        [SerializeField] TimeModificationManagerSettings _settings;

        TimeModParameter _timeSlowAllParam;
        TimeModParameter _timeFastAllParam;
        TimeModParameter _timeWithEnergyBoostParam;
        
        Coroutine _timeSpeedModificationRoutine;
        WaitUntil _waitForTimeModificationFinished;
        
        HashSet<IModifyTimeSpeedMultiplier> _objectsToModifySpeed;

        //Here I use Awake instead of Start to ensure that every value will be assigned before modifications.
        void Awake() => Initialise();

        void OnDestroy() {
            if (_timeSpeedModificationRoutine != null) 
                StopCoroutine(_timeSpeedModificationRoutine);
        }

        void Initialise() {
            _currentSpeedModifier = _settings.defaultTimeSpeedMultiplier;
            _waitForTimeModificationFinished = new WaitUntil(() => !IsModifyingSpeed && _energyBoostEnabled);
            _timeSlowAllParam = new TimeModParameter {
                duration = _settings.timeModSlowAllParam.duration,
                minTimeMulVal = _settings.timeModSlowAllParam.minTimeMultiplier,
                maxTimeMulVal = _settings.timeModSlowAllParam.maxTimeMultiplier
            };
            
            _timeFastAllParam = new TimeModParameter {
                duration = _settings.timeModFastAllParam.duration,
                minTimeMulVal = _settings.timeModFastAllParam.minTimeMultiplier,
                maxTimeMulVal = _settings.timeModFastAllParam.maxTimeMultiplier
            };
            
            _timeWithEnergyBoostParam = new TimeModParameter {
                duration = _settings.timeWithEnergyBoostModParam.duration,
                minTimeMulVal = _settings.timeWithEnergyBoostModParam.minTimeMultiplier,
                maxTimeMulVal = _settings.timeWithEnergyBoostModParam.maxTimeMultiplier
            };
        }

        protected override void HandleOnBonusEnabled(BonusSettings bonusSettings) {
            base.HandleOnBonusEnabled(bonusSettings);
            if (bonusSettings.bonusType == BonusType.TimeModeFastAll) {
                ApplyTimeModFastMultiplier();
            }
        }

        protected override void HandleOnBonusDisabled(BonusSettings bonusSettings) {
            base.HandleOnBonusDisabled(bonusSettings);
            if (bonusSettings.bonusType == BonusType.TimeModeFastAll) {
                SetDefaultTimeModMultiplier();
            }
        }

        protected override void ManageEnabledBonus() {
            //This check ensures that: WaitEnergyBoostTimeModification will complete correctly.
            //Also disables TimeFastAll mode.
            if (SIPlayerBonusesManager.IsBonusActive(BonusType.EnergyBoost))
                return;
            
            ApplyTimeModSlowMultiplier();
            ApplyTimeModFastMultiplier();
        }

        protected override void ManageDisabledBonus() {
            SetDefaultTimeModMultiplier();
        }

        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            SIGameplayEvents.OnSpeedModificationRequested += HandleOnSpeedModificationRequested;
            SIGameplayEvents.OnWaveCoolDown += HandleOnWaveCoolDown;
        }
        
        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            SIGameplayEvents.OnSpeedModificationRequested -= HandleOnSpeedModificationRequested;
            SIGameplayEvents.OnWaveCoolDown -= HandleOnWaveCoolDown;
        }

        void HandleOnSpeedModificationRequested(IModifyTimeSpeedMultiplier objToModifyTime) {
            if(_objectsToModifySpeed == null)
                _objectsToModifySpeed = new HashSet<IModifyTimeSpeedMultiplier>();

            if (_objectsToModifySpeed.Contains(objToModifyTime))
                return;
            
            _objectsToModifySpeed.Add(objToModifyTime);
            objToModifyTime.SetTimeSpeedModifier(_currentSpeedModifier);
        }
        
        void HandleOnWaveCoolDown() {
            SetDefaultTimeModMultiplier();
        }

        IEnumerator TimeSpeedModificationRoutine(TimeModParameter modParam, AnimationCurve curve) {
            _timeSpeedModificationProgress = 0.0f;
            float time = 0.0f;
            float modifierValue = _settings.defaultTimeSpeedMultiplier;
            IsModifyingSpeed = true;
            while (time < modParam.duration) {
                _timeSpeedModificationProgress = time / modParam.duration;
                modifierValue = Mathf.Lerp(modParam.fromTimeMul, modParam.toTimeMul, curve.Evaluate(_timeSpeedModificationProgress));
                modifierValue = Mathf.Clamp(modifierValue, modParam.minTimeMulVal, modParam.maxTimeMulVal);
                ManageObjectModifyingSpeed(modifierValue);
                time += Time.fixedDeltaTime;
                yield return WaitForUtils.SkipFixedFrames(1);
            }
            IsModifyingSpeed = false;
            _timeSpeedModificationProgress = 1f;
            _currentSpeedModifier = modParam.toTimeMul;
            ManageObjectModifyingSpeed(modParam.toTimeMul);
        }

        void ApplyTimeModFastMultiplier() {
            _timeFastAllParam.fromTimeMul = _currentSpeedModifier;
            _timeFastAllParam.toTimeMul = _settings.timeModFastAllParam.targetTimeMultiplier;
            ApplySpeedModification(_timeFastAllParam, _settings.speedUpCurve);
        }
        
        void ApplyTimeModSlowMultiplier() {
            _timeSlowAllParam.fromTimeMul = _currentSpeedModifier;
            _timeSlowAllParam.toTimeMul = _settings.timeModSlowAllParam.targetTimeMultiplier;
            ApplySpeedModification(_timeSlowAllParam, _settings.slowDownCurve);
        }
        
        void SetDefaultTimeModMultiplier() {
            _timeSlowAllParam.fromTimeMul = _currentSpeedModifier;
            _timeSlowAllParam.toTimeMul = _settings.defaultTimeSpeedMultiplier;
            ApplySpeedModification(_timeSlowAllParam, _settings.speedUpCurve);
        }
        
        [Button]
        void SetEnergyBoostSpeedModifierStartVal() {
            float slowDownMultiplier = _settings.timeModSlowAllParam.targetTimeMultiplier;
            _timeWithEnergyBoostParam.fromTimeMul = slowDownMultiplier;
            _timeWithEnergyBoostParam.toTimeMul = slowDownMultiplier * _settings.timeWithEnergyBoostModParam.targetTimeMultiplier;
            ApplySpeedModification(_timeWithEnergyBoostParam, _settings.slowDownCurve);
        }
        
        [Button]
        void SetEnergyBoostMultiplierEndVal() {
            float toModifier = SIPlayerBonusesManager.IsBonusActive(BonusType.TimeModSlowAll)
                ? _settings.timeModSlowAllParam.targetTimeMultiplier
                : _settings.defaultTimeSpeedMultiplier;

            _timeWithEnergyBoostParam.fromTimeMul = _currentSpeedModifier;
            _timeWithEnergyBoostParam.toTimeMul = toModifier;
            ApplySpeedModification(_timeWithEnergyBoostParam, _settings.speedUpCurve);
        }

        protected override void EnableEnergyBoostForBonus(bool isEnabled) {
            base.EnableEnergyBoostForBonus(isEnabled);
            _timeSpeedModificationRoutine = StartCoroutine(WaitEnergyBoostTimeModification(isEnabled));
        }

        IEnumerator WaitEnergyBoostTimeModification(bool isEnergyBoostEnabled) {
            yield return StartCoroutine(_waitForTimeModificationFinished);
            if (isEnergyBoostEnabled) {
                SetEnergyBoostSpeedModifierStartVal();
            }
            else {
                SetEnergyBoostMultiplierEndVal();
            }
        }

        void ApplySpeedModification(TimeModParameter modParam, AnimationCurve curve) {
            if (CanRestartTimeSpeedModificationRoutine(modParam.toTimeMul)) 
                _timeSpeedModificationRoutine = StartCoroutine(TimeSpeedModificationRoutine(modParam, curve));
            else if(!_settings.useIncrementalSpeedModification)
                ManageObjectModifyingSpeed(modParam.toTimeMul);
        }

        bool CanRestartTimeSpeedModificationRoutine(float multiplier) {
            return _settings.useIncrementalSpeedModification && !IsModifyingSpeed && 
                   Math.Abs(multiplier - _currentSpeedModifier) > 1e-05f;
        }

        void ManageObjectModifyingSpeed(float speedMultiplier) {
            float progressToUse = _settings.useIncrementalSpeedModification && IsModifyingSpeed
                ? _timeSpeedModificationProgress : 1f;
            foreach (IModifyTimeSpeedMultiplier objToModify in _objectsToModifySpeed) {
                objToModify.SetTimeSpeedModifier(speedMultiplier, progressToUse);
            }
        }
        
        //Buttons to test coroutines
        [Button]
        void DefaultTimeMod() {
            SetDefaultTimeModMultiplier();
        }

        [Button]
        void TimeModSlowAll() {
            ApplyTimeModSlowMultiplier();
        }
        
        [Button]
        void TimeModFastAll() {
            ApplyTimeModFastMultiplier();
        }
    }
}