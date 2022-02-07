using UnityEngine;

namespace SpaceInvaders {
    public class SIBonusMovement : MonoBehaviour, ICanMove, IModifyTimeSpeedMultiplier {

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

        void OnEnable() => SubscribeEvents();

        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents() {
            SIEventsHandler.OnUpdate += HandleOnUpdate;
        }

        void UnsubscribeEvents() {
            SIEventsHandler.OnUpdate -= HandleOnUpdate;
        }
        
        void HandleOnUpdate() {
            CheckIsInVerticalViewportSpace();
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
            SIGameplayEvents.BroadcastOnSpeedModificationRequested(this);
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
            _rigidbody.velocity = SIHelpers.VectorZero;
            _thisTransform.SetParent(_parent);
            _thisTransform.localPosition = SIScreenUtils.HiddenObjectPosition;
        }
        
        void CheckIsInVerticalViewportSpace() {
            
            if (!SIScreenUtils.IsInVerticalWorldScreenLimit(_thisTransform.position.y))
                StopObject();
        }
        
        Vector3 GetReleaseForce() {
            return SIHelpers.VectorDown * 
            (_releaseForceMultiplier * _currentReleaseForceModifier);
        }
    }
}