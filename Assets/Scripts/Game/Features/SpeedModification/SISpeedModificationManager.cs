using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders {
    public class SISpeedModificationManager : MonoBehaviour {
        [SerializeField] float _slowDownMultiplier;
        [SerializeField] float _speedUpMultiplier;
        [SerializeField] float _defaultSpeedMultiplier;
        
        HashSet<IModifySpeed> _objectsToModifySpeed;
        
        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents() {
            SIGameplayEvents.OnSpeedModificationRequested += HandleOnSpeedModificationRequested;
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
        }
        
        void UnsubscribeEvents() {
            SIGameplayEvents.OnSpeedModificationRequested -= HandleOnSpeedModificationRequested;
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
        }
        
        void HandleOnSpeedModificationRequested(IModifySpeed objToModify) {
            if(_objectsToModifySpeed == null)
                _objectsToModifySpeed = new HashSet<IModifySpeed>();

            if (!_objectsToModifySpeed.Contains(objToModify))
                _objectsToModifySpeed.Add(objToModify);
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

        void ApplySlowDownMultiplier() {
            ManageObjectToModifySpeed(_slowDownMultiplier);
        }

        void ApplySpeedUpMultiplier() {
            //For now intentionally uniplemented.
            // ManageObjectToModifySpeed(_speedUpMultiplier);
        }

        void SetDefaultSpeedMultiplier() {
            ManageObjectToModifySpeed(_defaultSpeedMultiplier);
        }

        void ManageObjectToModifySpeed(float speedMultiplier) {
            foreach (IModifySpeed objToModify in _objectsToModifySpeed) {
                objToModify.SetSpeedModifier(speedMultiplier);
            }
        }
    }
}