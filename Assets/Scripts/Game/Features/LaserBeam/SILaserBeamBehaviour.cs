using SpaceInvaders;

namespace Game.Features.LaserBeam {
    public class SILaserBeamBehaviour : SIBonusDrivenBehaviour {
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
    }
}