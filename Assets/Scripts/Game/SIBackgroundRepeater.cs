using UnityEngine;

namespace SpaceInvaders
{
    public class SIBackgroundRepeater : MonoBehaviour, ICanMove
    {
        [SerializeField] float _repeatDistance;
        [SerializeField] float _speed;

        Vector3 _startPosition;
        Vector3 _direction;
        Transform _cachedTransform;

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
            SIEventsHandler.OnNonPlayableUpdate += MoveObject;
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnNonPlayableUpdate -= MoveObject;
        }

        void Start()
        {
            Initialise();
        }

        void Initialise()
        {
            _cachedTransform = transform;
            _startPosition = _cachedTransform.position;
            float eulerAnglesZ = _cachedTransform.eulerAngles.z;

            _direction = new Vector3(
                Mathf.Cos(eulerAnglesZ * Mathf.Deg2Rad),
                Mathf.Sin(eulerAnglesZ * Mathf.Deg2Rad),
                0);
        }

        public void MoveObject()
        {
            float newPosition = Mathf.Repeat(Time.time * _speed, _repeatDistance);
            _cachedTransform.position = _startPosition + _direction * newPosition;
        }

        public void StopObject()
        {
            throw new System.NotImplementedException();
        }
    }
}