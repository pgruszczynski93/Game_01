using UnityEngine;

namespace SpaceInvaders
{
    public class SIAsteroidBehaviour : MonoBehaviour, IMoveable
    {
        [SerializeField] private float _minForce;
        [SerializeField] private float _maxForce;

        [SerializeField] private AsteroidState _asteroidState;
        [SerializeField] private Rigidbody _rigidbody;

        private bool _isMoving;
        private Transform _cachedTransform;
        private Vector3 _startPosition;
        private SIScreenController _screenController;
        private SIPlayerBehaviour _player;

        private void Start()
        {
            Initialise();
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
            SIEventsHandler.OnObjectsMovement += ManageObjectVisibility;
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnObjectsMovement -= ManageObjectVisibility;
        }

        private void Initialise()
        {
            if (_rigidbody == null)
            {
                Debug.LogError("No rigidbody attached.", this);
                return;
            }

            _asteroidState = AsteroidState.ReadyToMove;
            _cachedTransform = transform;
            _startPosition = _cachedTransform.localPosition;

            SIGameMasterBehaviour gmInstance = SIGameMasterBehaviour.Instance;
            _player = gmInstance.Player;
            _screenController = gmInstance.ScreenController;

            RotateTowardsScreen();
        }

        public void MoveObject()
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
            _cachedTransform.localRotation = lookRotation;
            Vector3 localEulerAngles = _cachedTransform.localEulerAngles;
            Vector3 eulerRotation =
                new Vector3(localEulerAngles.x, ConvertEulerY(localEulerAngles.y), localEulerAngles.z);
            _cachedTransform.localRotation = Quaternion.Euler(eulerRotation);
        }

        private float ConvertEulerY(float localEulerY)
        {
            if (localEulerY >= 0.0f && localEulerY <= 180.0f)
                return 90.0f;
            return 270.0f;
        }

        public void StopObject()
        {
            Reset();
        }

        void ManageObjectVisibility()
        {
            Vector3 currentPosition = _cachedTransform.localPosition;
            bool isObjectVisible = _screenController.IsInTheSpawnSpace(currentPosition);
            

            if (isObjectVisible == false && _asteroidState == AsteroidState.OnScreen)
            {
                StopObject();
                return;
            }

            if(isObjectVisible)
            {
                _asteroidState = AsteroidState.OnScreen;
            }
        }
    }
}