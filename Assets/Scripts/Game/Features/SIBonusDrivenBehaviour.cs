using UnityEngine;

namespace SpaceInvaders {
    public abstract class SIBonusDrivenBehaviour : MonoBehaviour {

        [SerializeField] protected BonusType _assignedBonusType;
        [SerializeField] protected GameObject _rootObject;

        protected bool _energyBoostEnabled;

        protected abstract void ManageEnabledBonus();
        protected abstract void ManageDisabledBonus();
        protected virtual void OnEnable() => SubscribeEvents();
        protected virtual void OnDisable() => UnsubscribeEvents();
        protected virtual void EnableEnergyBoostForBonus(bool isEnabled) {
            _energyBoostEnabled = isEnabled;
        }
        
        protected virtual void SubscribeEvents() {
            //OnIndependentUpdate is used to not press start every time
            SIEventsHandler.OnIndependentUpdate += HandleOnUpdate;
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
            SIGameplayEvents.OnWaveEnd += HandleOnWaveEnd;
        }

        protected virtual void UnsubscribeEvents() {
            SIEventsHandler.OnIndependentUpdate -= HandleOnUpdate;
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
            SIGameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
        }

        protected virtual void HandleOnBonusEnabled(BonusSettings bonusSettings) {
            if (bonusSettings.bonusType == _assignedBonusType)
                ManageEnabledBonus();
        }

        protected virtual void HandleOnBonusDisabled(BonusSettings bonusSettings) {
            if (bonusSettings.bonusType == _assignedBonusType)
                ManageDisabledBonus();
            if (bonusSettings.bonusType == BonusType.EnergyBoost)
                EnableEnergyBoostForBonus(false);
        }

        protected virtual void HandleOnUpdate() {
            CheckForEnergyBoostBonus();
        }
        
        protected virtual void HandleOnWaveEnd() {
            ManageDisabledBonus();
        }

        void CheckForEnergyBoostBonus() {
            if (!CanRunEnergyBoostBonus()) 
                return;

            Debug.Log($"{_assignedBonusType.ToString()} - energyboost");
            EnableEnergyBoostForBonus(true);
        }

        bool CanRunEnergyBoostBonus() {
            if (IsTimeModificationWithBoostToggled())
                return SIPlayerBonusesManager.IsBonusActive(BonusType.TimeModification);;
            
            return _rootObject != null && 
                   _rootObject.activeInHierarchy && 
                   !_energyBoostEnabled && 
                   SIPlayerBonusesManager.IsBonusActive(BonusType.EnergyBoost);
        }

        bool IsTimeModificationWithBoostToggled() {
            //Note: For TimeModification rootObject is null because it doesn't enable/disable any GO
            return _rootObject == null &&
                   !_energyBoostEnabled &&
                   _assignedBonusType == BonusType.TimeModification &&
                   SIPlayerBonusesManager.IsBonusActive(BonusType.EnergyBoost);
        }

        protected void EnableRootObject() {
            if (_rootObject == null) {
                Debug.Log($"{_assignedBonusType.ToString()} - no root object");
                return;
            }
            _rootObject.SetActive(true);
        }

        protected void DisableRootObject() {
            if (_rootObject == null)
                return;
            _rootObject.SetActive(false);
        }
    }
}