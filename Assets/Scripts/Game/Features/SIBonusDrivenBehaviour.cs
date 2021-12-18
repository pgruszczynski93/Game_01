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
        }

        protected virtual void UnsubscribeEvents() {
            SIEventsHandler.OnUpdate -= HandleOnUpdate;
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
        }

        void HandleOnUpdate() {
            CheckForEnergyBoostBonus();
        }

        void CheckForEnergyBoostBonus() {
            if (!CanRunExtraBoostBonus()) 
                return;

            Debug.Log($"{nameof(GetType)} _ EXTRA");
            ManageEnergyBoostBonus(true);
        }

        bool CanRunExtraBoostBonus() {
            return _rootObject.activeInHierarchy && !_energyBoostActive && SIPlayerBonusesManager.IsBonusActive(BonusType.EnergyBoost);
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
        
        protected void EnableRootObject() {
            _rootObject.SetActive(true);
        }

        protected void DisableRootObject() {
            _rootObject.SetActive(false);
        }
    }
}