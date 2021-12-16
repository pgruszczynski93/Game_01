using System.Collections;
using Configs;
using SpaceInvaders;
using UnityEngine;

namespace Game.Features.Shield {
    public class SIShieldBehaviour : SIBonusDrivenBehaviour{

        [SerializeField] ShieldSettings _shieldSettings;
        [SerializeField] SIShieldAnimatorController _animatorController;
        
        Coroutine _shieldAnimationRoutine;

        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            SIGameplayEvents.OnWaveEnd += HandleOnWaveEnd;
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
        }
        
        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            SIGameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
        }

        protected override void OnDisable() {
            base.OnDisable();
            if(_shieldAnimationRoutine != null)
                StopCoroutine(DisableRoutine());
        }
        
        void HandleOnWaveEnd() {
            ManageDisabledBonus();
        }

        void HandleOnBonusEnabled(BonusSettings bonusSettings) {
            switch(bonusSettings.bonusType) {
                case BonusType.Health:
                    break;
                case BonusType.Projectile:
                    break;
                case BonusType.ShieldSystem:
                    break;
                case BonusType.LaserBeam:
                    break;
                case BonusType.ExtraEnergy:
                    _animatorController.EnableExtraEnergyAnimation(true);
                    break;
                case BonusType.TimeSlowDown:
                    break;
            }
        }

        void HandleOnBonusDisabled(BonusSettings bonusSettings) {
            switch(bonusSettings.bonusType) {
                case BonusType.Health:
                    break;
                case BonusType.Projectile:
                    break;
                case BonusType.ShieldSystem:
                    break;
                case BonusType.LaserBeam:
                    break;
                case BonusType.ExtraEnergy:
                    _animatorController.EnableExtraEnergyAnimation(false);
                    break;
                case BonusType.TimeSlowDown:
                    break;
            }
        }

        protected override void ManageEnabledBonus() {
            EnableShield();
        }

        protected override void ManageDisabledBonus() {
            _shieldAnimationRoutine = StartCoroutine(DisableRoutine());
        }

        void EnableShield() {
            EnableRootObject();
            _animatorController.SetShowAnimation();
        }
        
        IEnumerator DisableRoutine() {
            yield return WaitUtils.WaitSecondsAndRunSequence(
                _animatorController.SetHideAnimation,     
                DisableRootObject, _shieldSettings.waitForDisableTime);
        }
    }
}