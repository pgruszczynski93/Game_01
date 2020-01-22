using UnityEngine;

namespace SpaceInvaders
{
    public class SITargetFollower : MonoBehaviour
    {
        [SerializeField] Vector3 _offsetFromTarget;
        [SerializeField] Transform _target;

        bool _initialised;
        Transform _thisTransform;
        
        void Initialise()
        {
            if (_initialised)
                return;

            _initialised = true;
            _thisTransform = transform;
        }

        void Start()
        {
            Initialise();
        }
        void OnEnable()
        {
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        void AssignEvents()
        {
            SIEventsHandler.OnUpdate += FollowTarget;
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnUpdate -= FollowTarget;
        }

        void FollowTarget()
        {
            _thisTransform.position = _target.position + _offsetFromTarget;
        }
    }
}