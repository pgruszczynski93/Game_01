using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class SISpeedModificationManager : MonoBehaviour {
        [SerializeField, Range(0, 3)] float _speedModificationDuration;
        [SerializeField, Range(0,1)] float _speedDownMultiplier;
        [SerializeField] float _speedUpMultiplier;
        [SerializeField] float _defaultSpeedMultiplier;

        bool isModifyingSpeed; 
        float _currentSpeedModifier;
        Coroutine _speedModificationRoutine;
        HashSet<IModifySpeed> _objectsToModifySpeed;

        void Start() => Initialise();
        
        void Initialise() {
            _currentSpeedModifier = _defaultSpeedMultiplier;
        }
        
        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents() {
            SIGameplayEvents.OnSpeedModificationRequested += HandleOnSpeedModificationRequested;
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
            SIGameplayEvents.OnWaveEnd += HandleOnWaveEnd;
        }
        
        void UnsubscribeEvents() {
            SIGameplayEvents.OnSpeedModificationRequested -= HandleOnSpeedModificationRequested;
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
            SIGameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
        }

        void HandleOnSpeedModificationRequested(IModifySpeed objToModify) {
            if(_objectsToModifySpeed == null)
                _objectsToModifySpeed = new HashSet<IModifySpeed>();

            if (_objectsToModifySpeed.Contains(objToModify))
                return;
            
            _objectsToModifySpeed.Add(objToModify);
            objToModify.SetSpeedModifier(_defaultSpeedMultiplier);
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
        
        void HandleOnWaveEnd() {
            SetDefaultSpeedMultiplier();
        }

        IEnumerator SpeedModificationRoutine(float targetSpeedModifier) {
            if (isModifyingSpeed)
                yield break;

            isModifyingSpeed = true;
            float time = 0.0f;
            float modifierValue = _defaultSpeedMultiplier;
            while (time < _speedModificationDuration) {
                time += Time.deltaTime;
                modifierValue = Mathf.Lerp(_currentSpeedModifier, targetSpeedModifier,
                    time / _speedModificationDuration);
                modifierValue = Mathf.Clamp(modifierValue, _speedDownMultiplier, _speedUpMultiplier);
                ManageObjectToModifySpeed(modifierValue);
                yield return WaitUtils.SkipFrames(1);
            }

            _currentSpeedModifier = modifierValue;
            isModifyingSpeed = false;
        }

        [Button]
        void Up() {
            StartCoroutine(SpeedModificationRoutine(1));
        }

        [Button]
        void Down() {
            StartCoroutine(SpeedModificationRoutine(0.3f));
        }

        void ApplySlowDownMultiplier() {
            _currentSpeedModifier = _speedDownMultiplier;
            ManageObjectToModifySpeed(_speedDownMultiplier);
        }

        void ApplySpeedUpMultiplier() {
            //For now intentionally uniplemented.
            // ManageObjectToModifySpeed(_speedUpMultiplier);
        }

        void SetDefaultSpeedMultiplier() {
            _currentSpeedModifier = _defaultSpeedMultiplier;
            ManageObjectToModifySpeed(_defaultSpeedMultiplier);
        }

        void ManageObjectToModifySpeed(float speedMultiplier) {
            foreach (IModifySpeed objToModify in _objectsToModifySpeed) {
                objToModify.SetSpeedModifier(speedMultiplier);
            }
        }
    }
}