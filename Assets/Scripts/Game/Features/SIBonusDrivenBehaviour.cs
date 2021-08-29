using UnityEngine;

namespace SpaceInvaders {
    public abstract class SIBonusDrivenBehaviour : MonoBehaviour {

        [SerializeField] protected BonusType _assignedBonusType;
        [SerializeField] protected GameObject _rootObject;

        protected abstract void ManageEnabledBonus();
        protected abstract void ManageDisabledBonus();

        protected virtual void OnEnable() => SubscribeEvents();

        protected virtual void OnDisable() => UnsubscribeEvents();
        
        protected virtual void SubscribeEvents() {
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
        }

        protected virtual void UnsubscribeEvents() {
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
        }

        void HandleOnBonusEnabled(BonusSettings bonusSettings) {
            if (bonusSettings.bonusType != _assignedBonusType)
                return;
            ManageEnabledBonus();
        }

        void HandleOnBonusDisabled(BonusSettings bonusSettings) {
            if (bonusSettings.bonusType != _assignedBonusType)
                return;

            ManageDisabledBonus();
        }
    }
}