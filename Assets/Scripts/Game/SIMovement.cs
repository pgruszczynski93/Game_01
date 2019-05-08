using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIMovement : MonoBehaviour
    {
        protected const float VERTICAL_MOVEMENT_VIEWPORT_STEP = 0.1f;
        protected float MAX_ROTATION_ANGLE;

        [SerializeField] protected bool _canObjectMove;
        [SerializeField] protected MovementType _movementType;
        [SerializeField] protected MovementDirection _movementDirection;
        [Range(0, 1)] [SerializeField] protected float _lerpStep;
        [SerializeField] protected float _currentMovementSpeed;

        private float _dt;
        [SerializeField] protected float _initialMovementSpeed;
        protected Vector2 _startPosition;
        protected Quaternion _fromRotation;
        protected Quaternion _toRotation;
        protected Transform _cachedTransform;
        protected Camera _mainCamera;

        protected Func<Vector3, Vector3> onScreenEdgeAction;

        public MovementType CurrentMovementType
        {
            set { _movementType = value; }
            get { return _movementType; }
        }

        protected void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            _mainCamera = SIGameMasterBehaviour.Instance.MainCamera;
            _cachedTransform = transform;
            _startPosition = _cachedTransform.localPosition;
            _initialMovementSpeed = _currentMovementSpeed;
        }

        protected virtual void ResetMovementProperties(){}

        protected virtual void OnEnable(){}

        protected virtual void OnDisable() {}

        //dorobic ruch pionowy 
        protected virtual void MoveObject(float movementValue, bool moveDownEnabled = false)
        {
            if (_mainCamera == null)
            {
                Debug.LogError("No camera attached");
                return;
            }

            _dt = Time.deltaTime;
            float horizontalMoveSpeed = _dt * movementValue * _currentMovementSpeed;

            Vector3 currentPosition = _cachedTransform.position;
            Vector3 newPosition = new Vector3(currentPosition.x + horizontalMoveSpeed, currentPosition.y);
            Vector3 smoothedPosition = Vector3.Lerp(currentPosition, newPosition, _lerpStep);

            Vector3 objectInCameraBoundsPos = _mainCamera.WorldToViewportPoint(smoothedPosition);
            objectInCameraBoundsPos.x = Mathf.Clamp(objectInCameraBoundsPos.x, SIHelpers.CAMERA_MIN_VIEWPORT_X,
                SIHelpers.CAMERA_MAX_VIEWPORT_X);

            //Debug.Log(objectInCameraBoundsPos.ToString("F4"));

            if (moveDownEnabled)
            {
                objectInCameraBoundsPos = onScreenEdgeAction.Invoke(objectInCameraBoundsPos);
            }

            objectInCameraBoundsPos = _mainCamera.ViewportToWorldPoint(objectInCameraBoundsPos);

            _cachedTransform.position = objectInCameraBoundsPos;
        }
    }
}
