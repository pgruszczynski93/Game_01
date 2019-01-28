using UnityEngine;

namespace SpaceInvaders
{
    public class SIMovement : MonoBehaviour
    {

        protected const float VERTICAL_MOVEMENT_VIEWPORT_STEP = 0.1f;

        [SerializeField] protected MovementType _movementType;
        [Range(0, 1)] [SerializeField] private float _lerpStep;

        [SerializeField] protected float _currentMovementSpeed;

        private float _dt;
        private Transform _cachedTransform;
        private Vector2 _startPosition;
        protected Camera _mainCamera;

        public MovementType CurrentMovementType
        {
            set { _movementType = value; }
            get { return _movementType; }
        }

        protected virtual void Awake()
        {
            SetInitialReferences();
        }

        protected virtual void SetInitialReferences()
        {
            _mainCamera = SIGameMasterBehaviour.Instance.MainCamera;
            _cachedTransform = transform;
            _startPosition = _cachedTransform.position;
        }

        protected virtual void OnEnable(){}

        protected virtual void OnDisable() {}

        protected virtual void MoveObject(float movementValue, bool moveDownEnabled = false)
        {
            if (_mainCamera == null)
            {
                Debug.LogError("No camera attached");
                return;
            }

            _dt = Time.deltaTime;
            float horizontalMoveSpeed = _dt * movementValue * _currentMovementSpeed;

            Vector2 currentPosition = _cachedTransform.position;
            Vector2 newPosition = new Vector2(currentPosition.x + horizontalMoveSpeed, currentPosition.y);
            Vector2 smoothedPosition = Vector2.Lerp(currentPosition, newPosition, _lerpStep);

            Vector2 objectInCameraBoundsPos = _mainCamera.WorldToViewportPoint(smoothedPosition);
            objectInCameraBoundsPos.x = Mathf.Clamp(objectInCameraBoundsPos.x, SIHelpers.CAMERA_MIN_VIEWPORT_X,
                SIHelpers.CAMERA_MAX_VIEWPORT_X);

            if (moveDownEnabled)
            {
                objectInCameraBoundsPos = TryToMoveObjectDown(objectInCameraBoundsPos);
            }

            objectInCameraBoundsPos = _mainCamera.ViewportToWorldPoint(objectInCameraBoundsPos);

            _cachedTransform.position = objectInCameraBoundsPos;
        }

        private Vector2 TryToMoveObjectDown(Vector2 objectInCameraBoundsPos)
        {
            if (objectInCameraBoundsPos.IsObjectInScreenHorizontalBounds2D())
            {
                objectInCameraBoundsPos.y -= VERTICAL_MOVEMENT_VIEWPORT_STEP;
                _currentMovementSpeed = -_currentMovementSpeed;
            }

            return objectInCameraBoundsPos;
        }
    }
}
