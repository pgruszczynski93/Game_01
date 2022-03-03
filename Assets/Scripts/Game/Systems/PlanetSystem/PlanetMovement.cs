using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class PlanetMovement : SIMovement {

        public float _speed;
        //todo: remove itlater - proto
        protected override void Initialise() {
            base.Initialise();
            _canMove = true;
        }


        protected override void TryToMoveObject() {
            base.TryToMoveObject();
            UpdatePosition();
        }

        protected override void UpdatePosition() {
            //proto !!
            _dt = Time.deltaTime;
            var curPos = _thisTransform.position;
            var deltaY = -1 * _dt * _speed;
            var newPos = new Vector3(curPos.x, curPos.y + deltaY, curPos.z);
            _thisTransform.position =  newPos;
        }

        protected override void UpdateRotation() {
            
        }
    }
}