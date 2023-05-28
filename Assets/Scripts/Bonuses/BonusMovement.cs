using PG.Game.Entities;
using PG.Game.EventSystem;
using PG.Game.Features;
using PG.Game.Helpers;
using PG.Game.Systems;
using UnityEngine;

namespace PG.Game.Bonuses {
    public class BonusMovement : MonoBehaviour, ICanMove, IModifyTimeSpeedMultiplier {
        [SerializeField] float _releaseForceMultiplier;
        [SerializeField] Rigidbody _rigidbody;
        [SerializeField] Transform _thisTransform;

        float _currentReleaseForceModifier;
        Vector3 _currentDropPos;
        Transform _parent;

        void Start() => Initialise();

        void Initialise() {
            _thisTransform = transform;
            _parent = _thisTransform.parent;
            _currentReleaseForceModifier = 1;
            RequestTimeSpeedModification();
        }

        public void SetDropPosition(Vector3 spawnPos) {
            _currentDropPos = spawnPos;
        }

        public void SetDropRotation(Vector3 spawnRot) {
            //intentionally unimplemented
        }

        public void SetTimeSpeedModifier(float timeSpeedModifier, float progress) {
            _currentReleaseForceModifier = timeSpeedModifier;
            _rigidbody.velocity = GetReleaseForce();
        }

        public void RequestTimeSpeedModification() {
            GameplayEvents.BroadcastOnSpeedModificationRequested(this);
        }

        public void MoveObject() {
            _thisTransform.parent = null;
            _thisTransform.position = _currentDropPos;
            if (_rigidbody.velocity.sqrMagnitude == 0)
                _rigidbody.AddForce(GetReleaseForce(), ForceMode.Impulse);
            else
                _rigidbody.velocity = GetReleaseForce();
        }

        public void StopObject() {
            _rigidbody.velocity = MathConsts.VectorZero;
            _thisTransform.SetParent(_parent);
            _thisTransform.localPosition = ScreenUtils.HiddenObjectPosition;
        }

        Vector3 GetReleaseForce() {
            return MathConsts.VectorDown * (_releaseForceMultiplier * _currentReleaseForceModifier);
        }
    }
}