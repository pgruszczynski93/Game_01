using System;
using System.Collections;
using System.Collections.Generic;
using Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class SITimeModificationManager : SIBonusDrivenBehaviour {
        
        [ShowInInspector] bool isModifyingSpeed; 
        [ShowInInspector] float _currentSpeedModifier;
        [ShowInInspector] float _timeSpeedModificationProgress;
        
        [SerializeField] TimeModificationManagerSettings _settings;

        TimeSpeedModificationParam _basicSlowDownParam;
        TimeSpeedModificationParam _energyBoostSlowDownParam;
        
        Coroutine _speedModificationRoutine;
        HashSet<IModifyTimeSpeedMultiplier> _objectsToModifySpeed;

        void Start() => Initialise();

        void OnDestroy() {
            if (_speedModificationRoutine != null) 
                StopCoroutine(_speedModificationRoutine);
        }

        void Initialise() {
            _basicSlowDownParam = new TimeSpeedModificationParam {
                duration = _settings.basicTimeMultiplierParam.duration,
                // fromTimeMul = _settings.basicTimeMultiplierParam.slowDownMultiplier,
                // toTimeMul = _settings.defaultTimeSpeedMultiplier,
                minTimeMulVal = _settings.basicTimeMultiplierParam.minMultiplier,
                maxTimeMulVal = _settings.basicTimeMultiplierParam.maxMultiplier
            };
            
            _energyBoostSlowDownParam = new TimeSpeedModificationParam {
                duration = _settings.energyBoostTimeMultiplierParam.duration,
                // fromTimeMul = _settings.energyBoostTimeMultiplierParam.slowDownMultiplier,
                // toTimeMul = _settings.defaultTimeSpeedMultiplier,
                minTimeMulVal = _settings.energyBoostTimeMultiplierParam.minMultiplier,
                maxTimeMulVal = _settings.energyBoostTimeMultiplierParam.maxMultiplier
            };
        }

        protected override void ManageEnabledBonus() {
            ApplySlowDownMultiplier();
        }

        protected override void ManageDisabledBonus() {
            SetDefaultSpeedMultiplier();
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
            objToModifyTime.SetTimeSpeedModifier(_settings.defaultTimeSpeedMultiplier);
        }
        
        void HandleOnWaveCoolDown() {
            SetDefaultSpeedMultiplier();
        }

        IEnumerator TimeSpeedModificationRoutine(TimeSpeedModificationParam modParam, AnimationCurve curve) {

            isModifyingSpeed = true;
            _timeSpeedModificationProgress = 0.0f;
            float time = 0.0f;
            float modifierValue = _settings.defaultTimeSpeedMultiplier;
            while (time < modParam.duration) {
                _timeSpeedModificationProgress = time / modParam.duration;
                modifierValue = Mathf.Lerp(modParam.fromTimeMul, modParam.toTimeMul, curve.Evaluate(_timeSpeedModificationProgress));
                modifierValue = Mathf.Clamp(modifierValue, modParam.minTimeMulVal, modParam.maxTimeMulVal);
                ManageObjectToModifySpeed(modifierValue);
                time += Time.fixedDeltaTime;
                yield return WaitUtils.SkipFixedFrames(1);
            }

            _timeSpeedModificationProgress = 1f;
            ManageObjectToModifySpeed(modParam.toTimeMul);
            _currentSpeedModifier = modParam.toTimeMul;
            isModifyingSpeed = false;
        }

        void ApplySpeedUpMultiplier() {
            //For now intentionally uniplemented.
            // ManageObjectToModifySpeed(_speedUpMultiplier);
        }
        
        void ApplySlowDownMultiplier() {
            _basicSlowDownParam.fromTimeMul = _currentSpeedModifier;
            _basicSlowDownParam.toTimeMul = _settings.basicTimeMultiplierParam.slowDownMultiplier;
            ApplySpeedModification(_basicSlowDownParam, _settings.slowDownCurve);
        }
        
        void SetDefaultSpeedMultiplier() {
            _basicSlowDownParam.fromTimeMul = _currentSpeedModifier;
            _basicSlowDownParam.toTimeMul = _settings.defaultTimeSpeedMultiplier;
            ApplySpeedModification(_energyBoostSlowDownParam, _settings.speedUpCurve);
        }
        
        [Button]
        void SetEnergyBoostSpeedModifierStartVal() {
            //todo opakować klase TimeSpeedMultiplierParameter w metody!!!
           // i dodać obsluge postprocesow 
            
            _energyBoostSlowDownParam.fromTimeMul = _currentSpeedModifier;
            _energyBoostSlowDownParam.toTimeMul = _currentSpeedModifier * _settings.energyBoostTimeMultiplierParam.slowDownMultiplier;
            ApplySpeedModification(_energyBoostSlowDownParam, _settings.slowDownCurve);
        }
        
        [Button]
        void SetEnergyBoostMultiplierEndVal() {
            float toModifier = SIPlayerBonusesManager.IsBonusActive(BonusType.TimeModification)
                ? _settings.basicTimeMultiplierParam.slowDownMultiplier
                : _settings.defaultTimeSpeedMultiplier;

            _energyBoostSlowDownParam.fromTimeMul = _currentSpeedModifier;
            _energyBoostSlowDownParam.toTimeMul = toModifier;
            ApplySpeedModification(_energyBoostSlowDownParam, _settings.speedUpCurve);
        }
        
        void ApplySpeedModification(TimeSpeedModificationParam modParam, AnimationCurve curve) {
            if (CanRestartTimeSpeedModificationRoutine(modParam.toTimeMul)) 
                _speedModificationRoutine = StartCoroutine(TimeSpeedModificationRoutine(modParam, curve));
            else if(!_settings.useIncrementalSpeedModification)
                ManageObjectToModifySpeed(modParam.toTimeMul);
        }

        bool CanRestartTimeSpeedModificationRoutine(float multiplier) {
            return _settings.useIncrementalSpeedModification && !isModifyingSpeed && 
                   Math.Abs(multiplier - _currentSpeedModifier) > 1e-05f;
        }

        void ManageObjectToModifySpeed(float speedMultiplier) {
            float progressToUse = _settings.useIncrementalSpeedModification && isModifyingSpeed
                ? _timeSpeedModificationProgress : 1f;
            foreach (IModifyTimeSpeedMultiplier objToModify in _objectsToModifySpeed) {
                objToModify.SetTimeSpeedModifier(speedMultiplier, progressToUse);
            }
        }
        
        //Buttons to test coroutines
        [Button]
        void SpeedUp() {
            SetDefaultSpeedMultiplier();
        }

        [Button]
        void SlowDown() {
            ApplySlowDownMultiplier();
        }
    }
}