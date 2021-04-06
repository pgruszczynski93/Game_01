using System.Collections;
using SpaceInvaders;
using UnityEngine;

namespace Game.Features.Shield {
    public class SIShieldBehaviour : MonoBehaviour {

        const float WAIT_TO_DISABLE = 0.5f;
        
        [SerializeField] GameObject _rootObject;
        [SerializeField] SIGameObjectVFX _shieldVfx;
        [SerializeField] SIShieldAnimatorController _animatorController;
        
        Coroutine _shieldAnimationRoutine;
        
        void OnEnable() => SubscribeEvents();

        void OnDisable() {
            if(_shieldAnimationRoutine != null)
                StopCoroutine(DisableRoutine());
            
            UnsubscribeEvents();
        } 

        void SubscribeEvents() {
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
        }

        void UnsubscribeEvents() {
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
        }

        void HandleOnBonusEnabled(BonusSettings bonusSettings) {
            if (bonusSettings.bonusType != BonusType.Shield)
                return;

            EnableShield();
        }

        void HandleOnBonusDisabled(BonusSettings bonusSettings) {
            if (bonusSettings.bonusType != BonusType.Shield)
                return;

            _shieldAnimationRoutine = StartCoroutine(DisableRoutine());
        }

        void EnableShield() {
            EnableBonus();
            _shieldVfx.TryToManageVFX(true, false, false);
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