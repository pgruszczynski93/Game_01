using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIMovement : MonoBehaviour
    {
        protected float BASIC_SPEED;

        protected const float CAMERA_MIN_PERCENT_OFFSET = 0.05f;
        protected const float VERTICAL_MOVEMENT_VIEWPORT_STEP = 0.1f;
        protected const float CAMERA_MAX_PERCENT_OFFSET = 0.95f;

        [SerializeField] protected MovementType _movementType;
        [Range(0, 1)] [SerializeField] private float _lerpStep;
        [SerializeField] protected Camera _mainCamera;

        [SerializeField] protected float _currentMovementSpeed;

        private float _dt;
        private Transform _cachedTransform;
        private Vector2 _startPosition;


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
            objectInCameraBoundsPos.x = Mathf.Clamp(objectInCameraBoundsPos.x, CAMERA_MIN_PERCENT_OFFSET,
                CAMERA_MAX_PERCENT_OFFSET);

            if (moveDownEnabled)
            {
                TryToMoveObjectDown(ref objectInCameraBoundsPos);
            }

            objectInCameraBoundsPos = _mainCamera.ViewportToWorldPoint(objectInCameraBoundsPos);

            _cachedTransform.position = objectInCameraBoundsPos;

        }

        private void TryToMoveObjectDown(ref Vector2 objectInCameraBoundsPos)
        {
            if ((objectInCameraBoundsPos.x >= CAMERA_MAX_PERCENT_OFFSET || objectInCameraBoundsPos.x <= CAMERA_MIN_PERCENT_OFFSET))
                //|| (_cachedTransform.localPosition.y >= CAMERA_MAX_PERCENT_OFFSET ||
                //    _cachedTransform.localPosition.y <= CAMERA_MIN_PERCENT_OFFSET))
            {
                objectInCameraBoundsPos.y -= VERTICAL_MOVEMENT_VIEWPORT_STEP;
                _currentMovementSpeed = -_currentMovementSpeed;
            }
        }
    }
}
