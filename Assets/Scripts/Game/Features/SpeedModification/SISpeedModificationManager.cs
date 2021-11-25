using System;
using System.Collections;
using System.Collections.Generic;
using Configs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class SISpeedModificationManager : MonoBehaviour {

        [SerializeField] SpeedModificationManagerSettings _settings;
        
        [ShowInInspector] bool isModifyingSpeed; 
        [ShowInInspector] float _currentSpeedModifier;
        [ShowInInspector] float _timeSpeedModificationProgress;
        Coroutine _speedModificationRoutine;
        HashSet<IModifyTimeSpeedMultiplier> _objectsToModifySpeed;

        void Start() => Initialise();

        void OnDestroy() {
            if (_speedModificationRoutine != null) 
                StopCoroutine(_speedModificationRoutine);
        }

        void Initialise() {
            _currentSpeedModifier = _settings.defaultSpeedMultiplier;       
        }
        
        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents() {
            SIGameplayEvents.OnSpeedModificationRequested += HandleOnSpeedModificationRequested;
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
            SIGameplayEvents.OnWaveCoolDown += HandleOnWaveCoolDown;
        }
        
        void UnsubscribeEvents() {
            SIGameplayEvents.OnSpeedModificationRequested -= HandleOnSpeedModificationRequested;
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
            SIGameplayEvents.OnWaveCoolDown -= HandleOnWaveCoolDown;
        }

        void HandleOnSpeedModificationRequested(IModifyTimeSpeedMultiplier objToModifyTime) {
            if(_objectsToModifySpeed == null)
                _objectsToModifySpeed = new HashSet<IModifyTimeSpeedMultiplier>();

            if (_objectsToModifySpeed.Contains(objToModifyTime))
                return;
            
            _objectsToModifySpeed.Add(objToModifyTime);
            objToModifyTime.SetTimeSpeedModifier(_settings.defaultSpeedMultiplier);
        }
        
        void HandleOnBonusEnabled(BonusSettings bonusSettings) {
            if (bonusSettings.bonusType == BonusType.TimeSlowDown) {
                ApplySlowDownMultiplier();
            }
        }
        
        void HandleOnBonusDisabled(BonusSettings bonusSettings) {
            if (bonusSettings.bonusType == BonusType.TimeSlowDown) {
                SetDefaultSpeedMultiplier();
            }
        }
        
        void HandleOnWaveCoolDown() {
            SetDefaultSpeedMultiplier();
        }

        IEnumerator TimeSpeedModificationRoutine(float targetSpeedModifier, AnimationCurve curve) {

            isModifyingSpeed = true;
            _timeSpeedModificationProgress = 0.0f;
            float time = 0.0f;
            float modifierValue = _settings.defaultSpeedMultiplier;
            while (time < _settings.speedModificationDuration) {
                _timeSpeedModificationProgress = time / _settings.speedModificationDuration;
                modifierValue = Mathf.Lerp(_currentSpeedModifier, targetSpeedModifier, curve.Evaluate(_timeSpeedModificationProgress));
                modifierValue = Mathf.Clamp(modifierValue, _settings.slowDownMultiplier, _settings.speedUpMultiplier);
                ManageObjectToModifySpeed(modifierValue);
                time += Time.fixedDeltaTime;
                yield return WaitUtils.SkipFixedFrames(1);
            }

            _timeSpeedModificationProgress = 1f;
            ManageObjectToModifySpeed(targetSpeedModifier);
            _currentSpeedModifier = targetSpeedModifier;
            isModifyingSpeed = false;
        }

        void ApplySpeedUpMultiplier() {
            //For now intentionally uniplemented.
            // ManageObjectToModifySpeed(_speedUpMultiplier);
        }
        
        void ApplySlowDownMultiplier() {
            ApplySpeedModification(_settings.slowDownMultiplier, _settings.slowDownCurve);
        }

        void SetDefaultSpeedMultiplier() {
            ApplySpeedModification(_settings.defaultSpeedMultiplier, _settings.speedUpCurve);
        }
        
        void ApplySpeedModification(float multiplier, AnimationCurve curve) {
            if (CanRestartTimeSpeedModificationRoutine(multiplier)) 
                _speedModificationRoutine = StartCoroutine(TimeSpeedModificationRoutine(multiplier, curve));
            else if(!_settings.useIncrementalSpeedModification)
                ManageObjectToModifySpeed(multiplier);
        }

        bool CanRestartTimeSpeedModificationRoutine(float multiplier) {
            return _settings.useIncrementalSpeedModification && !isModifyingSpeed &&
                   Math.Abs(multiplier - _currentSpeedModifier) > 1e-05f;
        }

        void ManageObjectToModifySpeed(float speedMultiplier) {
            float progressToUse = _settings.useIncrementalSpeedModification ? _timeSpeedModificationProgress : 1f;
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