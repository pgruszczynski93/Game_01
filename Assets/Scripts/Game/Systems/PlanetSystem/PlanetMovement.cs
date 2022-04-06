using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class PlanetMovement : SIMovement {

        Vector3 _movementDirection;
        
        public override void SetTimeSpeedModifier(float timeSpeedModifier, float progress = 1) {
            _speedModificator = timeSpeedModifier;
        }
        
        protected override void Initialise() {
            base.Initialise();
            _currentMovementSpeed = _initialMovementSpeed;
            _movementDirection = Vector3.down;
        }
        
        protected override bool IsMovementPossible() {
            return _canMove;
        }

        protected override void UpdatePosition() {
            _dt = Time.deltaTime;
            float vertMovementDelta = _dt * _currentMovementSpeed *_speedModificator * _movementDirection.y; 
            Vector3 currentPos = _thisTransform.position;
            Vector3 newPos = new Vector3(currentPos.x, currentPos.y + vertMovementDelta, currentPos.z);
            _thisTransform.position =  newPos;
        }

        protected override void UpdateRotation() {
            //Intentionally unimplemented. Rotation around it's own Y axis will be done by animation.s
        }
    }
}