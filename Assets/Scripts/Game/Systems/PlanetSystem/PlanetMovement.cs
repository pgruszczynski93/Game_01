using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class PlanetMovement : SIMovement {

        //todo - scriptable object do zaladowania ustawienia ruchu
        Vector3 _movementDirection;
        
        protected override void Initialise() {
            base.Initialise();
            _canMove = true;
            _initialMovementSpeed = 1;
            _currentMovementSpeed = _initialMovementSpeed;
            _movementDirection = Vector3.down;
        }
        
        protected override bool IsMovementPossible() {
            return _canMove;
        }

        protected override void UpdatePosition() {
            _dt = Time.deltaTime;
            float vertMovementDelta = _dt * _currentMovementSpeed * _speedModificator * _movementDirection.y; 
            Vector3 currentPos = _thisTransform.position;
            Vector3 newPos = new Vector3(currentPos.x, currentPos.y + vertMovementDelta, currentPos.z);
            _thisTransform.position =  newPos;
        }

        protected override void UpdateRotation() {
            //Intentionally unimplemented. Rotation around it's own Y axis will be done by animation.s
        }
    }
}