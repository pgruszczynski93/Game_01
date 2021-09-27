using System.Collections;
using SpaceInvaders;
using UnityEngine;

namespace Game.Features.Shield {
    public class SIShieldBehaviour : SIBonusDrivenBehaviour{

        const float WAIT_TO_DISABLE = 0.5f;
        
        [SerializeField] SIShieldAnimatorController _animatorController;
        
        Coroutine _shieldAnimationRoutine;

        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            SIGameplayEvents.OnWaveEnd += HandleOnWaveEnd;
        }
        
        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            SIGameplayEvents.OnWaveEnd += HandleOnWaveEnd;
        }

        protected override void OnDisable() {
            base.OnDisable();
            if(_shieldAnimationRoutine != null)
                StopCoroutine(DisableRoutine());
        }
        
        void HandleOnWaveEnd() {
            ManageDisabledBonus();
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
                DisableRootObject, WAIT_TO_DISABLE);
        }
    }
}