using System.Collections;
using SpaceInvaders;
using UnityEngine;

namespace Game.Features.Shield {
    public class SIShieldBehaviour : SIBonusDrivenBehaviour{

        const float WAIT_TO_DISABLE = 0.5f;
        
        [SerializeField] SIShieldAnimatorController _animatorController;
        
        Coroutine _shieldAnimationRoutine;
        
        protected override void OnDisable() {
            base.OnDisable();
            if(_shieldAnimationRoutine != null)
                StopCoroutine(DisableRoutine());
        }

        protected override void ManageEnabledBonus() {
            EnableShield();
        }

        protected override void ManageDisabledBonus() {
            _shieldAnimationRoutine = StartCoroutine(DisableRoutine());
        }

        void EnableShield() {
            EnableBonus();
            _animatorController.SetShowAnimation();
        }

        void EnableBonus() {
            _rootObject.SetActive(true);
        }

        void DisableBonus() {
            _rootObject.SetActive(false);
        }

        IEnumerator DisableRoutine() {
            yield return WaitUtils.WaitSecondsAndRunSequence(
                _animatorController.SetHideAnimation,     
                DisableBonus, WAIT_TO_DISABLE);
        }
    }
}