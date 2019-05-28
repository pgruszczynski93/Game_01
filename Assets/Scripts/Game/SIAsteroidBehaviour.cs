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
            Vector3 toPlayerDirection = (_player.transform.position - _startPosition).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(toPlayerDirection, Vector3.up);
            _cachedTransform.rotation = lookRotation;
            Debug.Log(toPlayerDirection);
            //naprawic rotacje asterodi
//            Vector3 localEulerAngles = _cachedTransform.localEulerAngles;
//            Vector3 eulerRotation = new Vector3(localEulerAngles.x, 90.0f, localEulerAngles.z);
//            _cachedTransform.rotation = Quaternion.Euler(eulerRotation);
        }

        public void MoveObj()
        {
            if (_isMoving)
            {
                return;
            }

            _rigidbody.AddForce(_cachedTransform.forward * 5f, ForceMode.Impulse);
            _isMoving = true;
        }

        private void Reset()
        {
            _isMoving = false;
            _rigidbody.velocity = SIHelpers.VectorZero;
            _asteroidState = AsteroidState.ReadyToMove;
            _cachedTransform.position = _startPosition;
            Vector3 toPlayerDirection = (_player.transform.position - _startPosition).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(toPlayerDirection, Vector3.up);
            _cachedTransform.rotation = lookRotation;
            Debug.Log(toPlayerDirection);
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
//            

        }
    }
}