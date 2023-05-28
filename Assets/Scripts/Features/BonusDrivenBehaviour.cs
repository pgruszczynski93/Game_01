using PG.Game.Configs;
using PG.Game.Entities.Player;
using PG.Game.EventSystem;
using PG.Game.Helpers;
using UnityEngine;
using WaveType = PG.Game.Systems.WaveSystem.WaveType;

namespace PG.Game.Features {
    public abstract class BonusDrivenBehaviour : MonoBehaviour {
        [SerializeField] protected BonusType _assignedBonusType;
        [SerializeField] protected GameObject _rootObject;

        protected bool _energyBoostEnabled;

        protected abstract void ManageEnabledBonus();
        protected abstract void ManageDisabledBonus();

        protected virtual void OnEnable() {
            SubscribeEvents();
        }

        protected virtual void OnDisable() {
            UnsubscribeEvents();
        }

        protected virtual void EnableEnergyBoostForBonus(bool isEnabled) {
            _energyBoostEnabled = isEnabled;
        }

        protected virtual void SubscribeEvents() {
            //OnIndependentUpdate is used to not press start every time
            GeneralEvents.OnIndependentUpdate += HandleOnUpdate;
            BonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            BonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
            GameplayEvents.OnWaveEnd += HandleOnWaveEnd;
        }

        protected virtual void UnsubscribeEvents() {
            GeneralEvents.OnIndependentUpdate -= HandleOnUpdate;
            BonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            BonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
            GameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
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

        protected virtual void HandleOnWaveEnd(WaveType waveType) {
            //Todo: Consider usage of waveType
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
                return PlayerBonusesManager.IsBonusActive(BonusType.TimeModSlowAll);
            ;

            return _rootObject != null &&
                   _rootObject.activeInHierarchy &&
                   !_energyBoostEnabled &&
                   PlayerBonusesManager.IsBonusActive(BonusType.EnergyBoost);
        }

        bool IsTimeModificationWithBoostToggled() {
            //Note: For TimeModification rootObject is null because it doesn't enable/disable any GO
            return _rootObject == null &&
                   !_energyBoostEnabled &&
                   _assignedBonusType == BonusType.TimeModSlowAll &&
                   PlayerBonusesManager.IsBonusActive(BonusType.EnergyBoost);
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