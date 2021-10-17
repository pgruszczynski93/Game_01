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
        Coroutine _speedModificationRoutine;
        HashSet<IModifySpeed> _objectsToModifySpeed;

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

        void HandleOnSpeedModificationRequested(IModifySpeed objToModify) {
            if(_objectsToModifySpeed == null)
                _objectsToModifySpeed = new HashSet<IModifySpeed>();

            if (_objectsToModifySpeed.Contains(objToModify))
                return;
            
            _objectsToModifySpeed.Add(objToModify);
            objToModify.SetSpeedModifier(_settings.defaultSpeedMultiplier);
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

        IEnumerator SpeedModificationRoutine(float targetSpeedModifier, AnimationCurve curve) {

            isModifyingSpeed = true;
            float time = 0.0f;
            float modifierValue = _settings.defaultSpeedMultiplier;
            while (time < _settings.speedModificationDuration) {
                time += Time.deltaTime;
                modifierValue = Mathf.Lerp(_currentSpeedModifier, targetSpeedModifier,
                    curve.Evaluate(time /_settings.speedModificationDuration));
                modifierValue = Mathf.Clamp(modifierValue, _settings.slowDownMultiplier, _settings.speedUpMultiplier);
                ManageObjectToModifySpeed(modifierValue);
                yield return WaitUtils.SkipFrames(1);
            }

            _currentSpeedModifier = modifierValue;
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
            if (_settings.useIncrementalSpeedModification && !isModifyingSpeed)
                _speedModificationRoutine = StartCoroutine(SpeedModificationRoutine(multiplier, curve));
            else
                ManageObjectToModifySpeed(multiplier);
        }

        void ManageObjectToModifySpeed(float speedMultiplier) {
            foreach (IModifySpeed objToModify in _objectsToModifySpeed) {
                objToModify.SetSpeedModifier(speedMultiplier);
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