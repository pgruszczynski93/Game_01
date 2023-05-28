using PG.Game.Entities;
using PG.Game.EventSystem;
using PG.Game.Features;
using PG.Game.Helpers;
using UnityEngine;

namespace PG.Game.Weapons.Projectile {
    public class ProjectileMovement : MonoBehaviour, ICanMove, IModifyTimeSpeedMultiplier {
        [SerializeField] Rigidbody _rigidbody;

        bool _isMoving;
        float _currentVelocityModifier;
        float _releaseForceMultiplier;
        Vector3 _moveDirection;
        Vector3 _parentRelativeLocalPos;
        Transform _thisTransform;
        ProjectileOwnerTag _ownerTag;

        public void Initialise(ProjectileOwnerTag ownerTag, float releaseForceMultiplier) {
            _isMoving = false;
            _ownerTag = ownerTag;
            _thisTransform = transform;
            _parentRelativeLocalPos = _thisTransform.localPosition;
            _currentVelocityModifier = 1f;
            _releaseForceMultiplier = releaseForceMultiplier;
            RequestTimeSpeedModification();
        }

        public void RequestTimeSpeedModification() {
            GameplayEvents.BroadcastOnSpeedModificationRequested(this);
        }

        public void SetTimeSpeedModifier(float timeSpeedModifier, float progress = 1) {
            _currentVelocityModifier = _ownerTag == ProjectileOwnerTag.Enemy ? timeSpeedModifier : _currentVelocityModifier;
            Vector3 releaseForce = GetReleaseForce();
            _rigidbody.velocity = releaseForce;
        }

        public void SetMovePosition(Vector3 pos) {
            _thisTransform.position = pos;
        }

        public void SetMoveDirection(Vector3 rotDirection) {
            _thisTransform.rotation = Quaternion.LookRotation(rotDirection, Vector3.forward);
            _moveDirection = rotDirection;
        }

        Vector3 GetReleaseForce() {
            float forceModifier = _releaseForceMultiplier * _currentVelocityModifier;
            Vector3 releaseForce = _moveDirection * forceModifier;
            return releaseForce;
        }

        public void MoveObject() {
            if (_isMoving)
                return;

            _isMoving = true;
            _thisTransform.parent = null;

            //Note: This if secures rigidbody from applying additional force when actually has velocity.
            //Objects are affected by timeSpeedModification so even if they're in pool their velocity is being modified.
            //This line would add extra force to their velocity. Same solution in Bonus class

            if (_rigidbody.velocity.sqrMagnitude == 0)
                _rigidbody.AddForce(GetReleaseForce(), ForceMode.Impulse);
            else
                _rigidbody.velocity = GetReleaseForce();
        }

        public void StopObject() {
            _isMoving = false;
            _thisTransform.localPosition = _parentRelativeLocalPos;
            _rigidbody.velocity = MathConsts.VectorZero;
            _rigidbody.angularVelocity = MathConsts.VectorZero;
        }

        public void SetParent(Transform parent) {
            _thisTransform.parent = parent;
        }
    }
}