using PG.Game.EventSystem;
using UnityEngine;

namespace PG.Game.Entities {
    public class TargetFollower : MonoBehaviour {
        [SerializeField] Vector3 _offsetFromTarget;
        [SerializeField] Transform _target;

        bool _initialised;
        Transform _thisTransform;

        void Initialise() {
            if (_initialised)
                return;

            _initialised = true;
            _thisTransform = transform;
        }

        void Start() {
            Initialise();
        }

        void OnEnable() {
            AssignEvents();
        }

        void OnDisable() {
            RemoveEvents();
        }

        void AssignEvents() {
            GeneralEvents.OnUpdate += FollowTarget;
        }

        void RemoveEvents() {
            GeneralEvents.OnUpdate -= FollowTarget;
        }

        void FollowTarget() {
            _thisTransform.position = _target.position + _offsetFromTarget;
        }
    }
}