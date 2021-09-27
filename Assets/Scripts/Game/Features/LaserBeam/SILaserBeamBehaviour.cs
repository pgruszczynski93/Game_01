using SpaceInvaders;

namespace Game.Features.LaserBeam {
    public class SILaserBeamBehaviour : SIBonusDrivenBehaviour {
        
        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            SIGameplayEvents.OnWaveEnd += HandleOnWaveEnd;
        }
        
        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            SIGameplayEvents.OnWaveEnd += HandleOnWaveEnd;
        }

        protected override void ManageEnabledBonus() {
            EnableLaserBeam();
        }

        protected override void ManageDisabledBonus() {
            DisableLaserBeam();
        }

        void EnableLaserBeam() {
            EnableRootObject();
        }

        void DisableLaserBeam() {
            DisableRootObject();
        }
        
        void HandleOnWaveEnd() {
            ManageDisabledBonus();
        }
    }
}