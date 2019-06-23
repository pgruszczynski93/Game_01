using UnityEngine;

namespace SpaceInvaders
{
    public class SIBackgroundRepeater : MonoBehaviour, IMoveable
    {
        [SerializeField] private float _repeatDistance;
        [SerializeField] private float _speed;

        private Vector3 _startPosition;
        private Vector3 _direction;
        private Transform _cachedTransform;

        private void OnEnable()
        {
            AssignEvents();
        }
        
        private void OnDisable()
        {
            RemoveEvents();
        }

        private void AssignEvents()
        {
            SIEventsHandler.OnGameIndependentObjectsMovement += MoveObject;
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnGameIndependentObjectsMovement -= MoveObject;
        }

        private void Start()
        {
            Initialise();
        }

        private void Initialise()
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
            _cachedTransform.position = _startPosition + (_direction * newPosition);
        }

        public void StopObject()
        {
            throw new System.NotImplementedException();
        }
    }
}