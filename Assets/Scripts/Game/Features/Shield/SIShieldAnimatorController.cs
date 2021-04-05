using UnityEngine;

namespace Game.Features.Shield {
    public class SIShieldAnimatorController : MonoBehaviour {
        static readonly int HiddenAnimationID = Animator.StringToHash("ShieldHidden");
        [SerializeField] Animator _shieldAnimator;

        public void SetShowAnimation() {
            _shieldAnimator.ResetTrigger(HiddenAnimationID);
        }
        
        public void SetHideAnimation() {
            _shieldAnimator.SetTrigger(HiddenAnimationID);
        }
    }
}