using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class PlanetMovement : SIMovement, ICanMove, IModifyTimeSpeedMultiplier {
        
        //todo: remove itlater - proto
        protected override void Initialise() {
            base.Initialise();
            _canMove = true;
        }


        protected override void TryToMoveObject() {
            base.TryToMoveObject();
            Debug.Log("planeta");

        }

        protected override void UpdatePosition() {
        }

        protected override void UpdateRotation() {
        }

        public void MoveObject() {
            
        }

        public void StopObject() {
        }


        public void RequestTimeSpeedModification() {
        }

        public void SetTimeSpeedModifier(float timeSpeedModifier, float progress = 1) {
        }
    }
}