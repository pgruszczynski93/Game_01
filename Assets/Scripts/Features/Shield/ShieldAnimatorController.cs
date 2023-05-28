using UnityEngine;

namespace PG.Game.Features.Shield {
    public class ShieldAnimatorController : MonoBehaviour {
        static readonly int HiddenAnimationID = Animator.StringToHash("ShieldHidden");
        static readonly int ExtraEnergyAnimationID = Animator.StringToHash("ExtraEnergyEnabled");

        [SerializeField] Animator _shieldAnimator;

        public void SetShowAnimation() {
            if (_shieldAnimator.isActiveAndEnabled)
                _shieldAnimator.ResetTrigger(HiddenAnimationID);
        }

        public void SetHideAnimation() {
            if (_shieldAnimator.isActiveAndEnabled)
                _shieldAnimator.SetTrigger(HiddenAnimationID);
        }

        public void EnableExtraEnergyAnimation(bool isEnabled) {
            if (_shieldAnimator.isActiveAndEnabled)
                _shieldAnimator.SetBool(ExtraEnergyAnimationID, isEnabled);
        }
    }
}