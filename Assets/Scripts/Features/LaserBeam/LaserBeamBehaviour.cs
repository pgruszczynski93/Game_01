using SpaceInvaders;
using UnityEngine;

namespace PG.Game.Features.LaserBeam  {
    public class LaserBeamBehaviour : BonusDrivenBehaviour {

        [SerializeField] LaserBeamDamage _laserDamage;
        
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

        protected override void EnableEnergyBoostForBonus(bool isEnabled) {
            base.EnableEnergyBoostForBonus(isEnabled);
            if(isEnabled)
                _laserDamage.EnableEnergyBoost();
            else 
                _laserDamage.DisableEnergyBoost();
        }

        protected override void HandleOnUpdate() {
            base.HandleOnUpdate();
            _laserDamage.DetectLaserHit();
        }
    }
}