using UnityEngine;

namespace SpaceInvaders {
    public abstract class SIBonusDrivenBehaviour : MonoBehaviour {

        [SerializeField] protected BonusType _assignedBonusType;
        [SerializeField] protected GameObject _rootObject;

        protected bool _energyBoostActive;

        protected abstract void ManageEnabledBonus();
        protected abstract void ManageDisabledBonus();
        protected virtual void OnEnable() => SubscribeEvents();
        protected virtual void OnDisable() => UnsubscribeEvents();
        protected virtual void ManageEnergyBoostBonus(bool isEnabled) {
            _energyBoostActive = isEnabled;
        }
        
        protected virtual void SubscribeEvents() {
            SIEventsHandler.OnUpdate += HandleOnUpdate;
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
            SIGameplayEvents.OnWaveEnd += HandleOnWaveEnd;
        }

        protected virtual void UnsubscribeEvents() {
            SIEventsHandler.OnUpdate -= HandleOnUpdate;
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
                ManageEnergyBoostBonus(false);
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
            ManageEnergyBoostBonus(true);
        }

        bool CanRunEnergyBoostBonus() {
            if (IsTimeModificationWithBoostToggled())
                return true;
            
            return _rootObject != null && 
                   _rootObject.activeInHierarchy && 
                   !_energyBoostActive && 
                   SIPlayerBonusesManager.IsBonusActive(BonusType.EnergyBoost);
        }

        bool IsTimeModificationWithBoostToggled() {
            return !_energyBoostActive &&
                   SIPlayerBonusesManager.IsBonusActive(BonusType.EnergyBoost) &&
                   SIPlayerBonusesManager.IsBonusActive(BonusType.TimeModification);
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