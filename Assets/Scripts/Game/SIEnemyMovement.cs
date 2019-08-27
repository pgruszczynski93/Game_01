using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyMovement : SIMovement, IMoveable
    {
        private int _rotationDirection;
        private float _speedIncrementValue;
        private float _currentMovementValueMultiplier;
        [SerializeField] private float _movementValueMultiplier;

        private Quaternion _enemyObjectOrientation;
        [SerializeField] private QuaternionTweenInfo _tweenInfo;

        protected override void Initialize()
        {
            base.Initialize();

            ResetMovementProperties();
        }

        protected override void ResetMovementProperties()
        {
            base.ResetMovementProperties();
            
            SIHelpers.SISimpleLogger(this, "<color=blue>Reset enemy</color>", SimpleLoggerTypes.Log);

            MAX_ROTATION_ANGLE = 20;
            _movementValueMultiplier = 1.5f;
            _currentMovementValueMultiplier = _movementValueMultiplier;
            _speedIncrementValue = 0.2f;
            _rotationDirection = 1;
            _enemyObjectOrientation = Quaternion.Euler(90, 0, 180);
            _cachedTransform.localRotation = _enemyObjectOrientation;
            _currentMovementSpeed = _initialMovementSpeed;
            _canObjectMove = true;
        }

        protected override void OnEnable()
        {
            SIEventsHandler.OnUpdate += MoveObject;
            SIEventsHandler.OnEnemySpeedMultiplierChanged += UpdateMovementStep;
            SIEventsHandler.OnWaveEnd += ResetEnemy;
            onScreenEdgeAction += TryToMoveObjectDown;
        }

        protected override void OnDisable()
        {
            SIEventsHandler.OnUpdate -= MoveObject;
            SIEventsHandler.OnEnemySpeedMultiplierChanged -= UpdateMovementStep;
            SIEventsHandler.OnWaveEnd -= ResetEnemy;

            onScreenEdgeAction -= TryToMoveObjectDown;
        }

        public void MoveObject()
        {
            if (SIEnemiesGridsMaster.Instance.IsEnemyInGridMovementAllowed == false || _canObjectMove == false)
            {
                return;
            }

            MoveObject(_currentMovementValueMultiplier, true);

        }

        private Vector3 TryToMoveObjectDown(Vector3 objectInCameraBoundsPos)
        {
            if (!objectInCameraBoundsPos.IsObjectOutOfHorizontalViewportBounds3D())
            {
                return objectInCameraBoundsPos;
            }
            
            objectInCameraBoundsPos.y -= VERTICAL_MOVEMENT_VIEWPORT_STEP;

            StopAllCoroutines();
            SetMovementDirectionProperties();
            StartCoroutine(RotateRoutine());

            return objectInCameraBoundsPos;
        }

        private void SetMovementDirectionProperties()
        {
            _currentMovementSpeed = -_currentMovementSpeed;
            _currentMovementValueMultiplier += _speedIncrementValue;

            if (_currentMovementSpeed > 0)
            {
                _movementDirection = MovementDirection.Right;
                _rotationDirection = 1;
            }
            else
            {
                _movementDirection = MovementDirection.Left;
                _rotationDirection = -1;
            }
        }

        private void SetTweenInfoValues(float zAngle = 0.0f)
        {
            _tweenInfo.startValue = _cachedTransform.rotation;
            _tweenInfo.endValue = Quaternion.Euler(90, 0, 180 + zAngle);
        }


        private IEnumerator RotateRoutine()
        {
            SetTweenInfoValues(_rotationDirection * MAX_ROTATION_ANGLE);

            yield return StartCoroutine(SIHelpers.SimpleTween3D((outQuaternion) =>
                {
                    _cachedTransform.rotation = outQuaternion;
                }, _tweenInfo, null));

        }

        private void UpdateMovementStep(float newStep)
        {
            _currentMovementValueMultiplier = newStep;
        }

        private void ResetEnemy()
        {
            _cachedTransform.localPosition = _startPosition;
            _currentMovementValueMultiplier = _movementValueMultiplier;

            ResetMovementProperties();
        }

        public void StopObject()
        {
            _canObjectMove = false;
        }
    }
}
