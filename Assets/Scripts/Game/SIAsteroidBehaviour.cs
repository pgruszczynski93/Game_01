using UnityEngine;

namespace SpaceInvaders
{
    public class SIAsteroidBehaviour : MonoBehaviour, IMoveable
    {
        private bool _isMoving;
        private Transform _cachedTransform;
        private Vector3 _startPosition;
        private Camera _mainCamera;
        private SIPlayerBehaviour _player;

        [SerializeField] private float _minForce;
        [SerializeField] private float _maxForce;
        
        [SerializeField] private AsteroidState _asteroidState;
        [SerializeField] private Rigidbody _rigidbody;

        private void Start()
        {
            Initialize();
        }

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
            SIEventsHandler.OnObjectsMovement += CheckIsObjectVisibleOnScreen;
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnObjectsMovement -= CheckIsObjectVisibleOnScreen;
        }

        private void Initialize()
        {
            if (_rigidbody == null)
            {
                Debug.LogError("No rigidbody attached.", this);
            }

            _asteroidState = AsteroidState.ReadyToMove;
            _cachedTransform = transform;
            _startPosition = _cachedTransform.localPosition;
            _mainCamera = SIGameMasterBehaviour.Instance.MainCamera;
            _player = SIGameMasterBehaviour.Instance.Player;
            RotateTowardsScreen();
        }

        public void MoveObj()
        {
            if (_isMoving)
            {
                return;
            }

            float forceMultiplier = Random.Range(_minForce, _maxForce);
            Vector3 forward = _cachedTransform.forward;
            Vector3 multipliedMoveVector = forward * forceMultiplier;
            _rigidbody.AddForce(multipliedMoveVector, ForceMode.Impulse);
            _rigidbody.AddTorque(multipliedMoveVector, ForceMode.Impulse);
            _isMoving = true;
        }

        private void Reset()
        {
            _isMoving = false;
            _rigidbody.velocity = SIHelpers.VectorZero;
            _rigidbody.angularVelocity = SIHelpers.VectorZero;
            _asteroidState = AsteroidState.ReadyToMove;
            _cachedTransform.localPosition = _startPosition;
            RotateTowardsScreen();
        }

        private void RotateTowardsScreen()
        {
            Vector3 toPlayerDirection = (_player.transform.localPosition - _startPosition).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(toPlayerDirection, Vector3.up);
//            _cachedTransform.localRotation = lookRotation;
//            Vector3 localEulerAngles = _cachedTransform.localEulerAngles;
//            Vector3 eulerRotation = new Vector3(localEulerAngles.x, ConvertEulerY(localEulerAngles.y), localEulerAngles.z);
//            _cachedTransform.localRotation = Quaternion.Euler(eulerRotation);
        }

        private float ConvertEulerY(float localEulerY)
        {
            if (localEulerY >= 0.0f && localEulerY <= 180.0f)
                return 90.0f;
            return 270.0f;
        }

        public void StopObj()
        {
            Reset();
        }

        void CheckIsObjectVisibleOnScreen()
        {
            Vector3 currentPosition = _cachedTransform.localPosition;
            Vector3 viewportPosition = _mainCamera.WorldToViewportPoint(currentPosition);
            bool isObjectVisible = viewportPosition.IsObjectVisibleInTheScreen();
            
            if (isObjectVisible == false && _asteroidState == AsteroidState.OnScreen)
            {
                StopObj();
                return;
            }

            if (isObjectVisible)
            {
                _asteroidState = AsteroidState.OnScreen;
            }
        }
    }
}