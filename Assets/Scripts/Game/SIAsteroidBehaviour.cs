using UnityEngine;

namespace SpaceInvaders
{
    public class SIAsteroidBehaviour : MonoBehaviour, IMoveable
    {
        private bool _isMoving;
        private Transform _cachedTransform;
        private Vector3 _startPosition;

        private SIPlayerBehaviour _player;
        private Camera _mainCamera;

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
            _startPosition = _cachedTransform.position;
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

            Vector3 forward = _cachedTransform.forward;
            float forceMultiplier = Random.Range(_minForce, _maxForce);
            _rigidbody.AddForce(forward * forceMultiplier, ForceMode.Impulse);
            _rigidbody.AddTorque(forward * forceMultiplier, ForceMode.Impulse);
            _isMoving = true;
        }

        private void Reset()
        {
            _isMoving = false;
            _rigidbody.velocity = SIHelpers.VectorZero;
            _asteroidState = AsteroidState.ReadyToMove;
            _cachedTransform.position = _startPosition;
            RotateTowardsScreen();
        }

        private void RotateTowardsScreen()
        {
            //currently rotates towards player
            Vector3 toPlayerDirection = (_player.transform.position - _startPosition).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(toPlayerDirection, Vector3.up);
            _cachedTransform.rotation = lookRotation;
            //naprawić rotacje - wprwadzic kąty 90;270
//            Vector3 localEulerAngles = _cachedTransform.localEulerAngles;
//            Vector3 eulerRotation = new Vector3(localEulerAngles.x, 90.0f, localEulerAngles.z);
//            _cachedTransform.rotation = Quaternion.Euler(eulerRotation);
        }

        public void StopObj()
        {
            Reset();
        }

        void CheckIsObjectVisibleOnScreen()
        {
            Vector3 currentPosition = _cachedTransform.position;
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