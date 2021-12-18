using System.Collections;
using Configs;
using SpaceInvaders;
using UnityEngine;

namespace Game.Features.Shield {
    public class SIShieldBehaviour : SIBonusDrivenBehaviour{

        [SerializeField] ShieldSettings _shieldSettings;
        [SerializeField] SIShieldAnimatorController _animatorController;

        Coroutine _shieldAnimationRoutine;

        protected override void OnDisable() {
            base.OnDisable();
            if(_shieldAnimationRoutine != null)
                StopCoroutine(DisableRoutine());
        }
        
        protected override void ManageEnergyBoostBonus(bool isEnabled) {
            base.ManageEnergyBoostBonus(isEnabled);
            _animatorController.EnableExtraEnergyAnimation(isEnabled);
        }

        protected override void ManageEnabledBonus() {
            EnableShield();
        }

        protected override void ManageDisabledBonus() {
            DisableShield();
        }

        void EnableShield() {
            EnableRootObject();
            _animatorController.SetShowAnimation();
        }

        void DisableShield() {
            _shieldAnimationRoutine = StartCoroutine(DisableRoutine());
        }

        IEnumerator DisableRoutine() {
            yield return WaitUtils.WaitSecondsAndRunSequence(
                _animatorController.SetHideAnimation,     
                DisableRootObject, _shieldSettings.waitForDisableTime);
        }
    }
}