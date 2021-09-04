using SpaceInvaders;
using UnityEngine;

namespace Game.Features.LaserBeam {
    public class SILaserBeamBehaviour : SIBonusDrivenBehaviour {

        [SerializeField] SILaserBeamDamage _laserBeamDamage;
        
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