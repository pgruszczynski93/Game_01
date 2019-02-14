using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyMovement : SIMovement, IMoveable
    {
        private bool _canEnemyMove;
        private int _rotationDirection;
        private float _moveStep;
        private Quaternion _enemyObjectOrientation;

        [SerializeField] private QuaternionTweenInfo _tweenInfo;

        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();

            ResetMovementProperties();
        }


        protected override void ResetMovementProperties()
        {
            base.ResetMovementProperties();
            
            SIHelpers.SISimpleLogger(this, "<color=blue>Reset enemy</color>", SimpleLoggerTypes.Log);

            MAX_ROTATION_ANGLE = 20;
            _moveStep = 2;
            _rotationDirection = 1;
            _enemyObjectOrientation = Quaternion.Euler(90, 0, 180);
            _cachedTransform.localRotation = _enemyObjectOrientation;
            _currentMovementSpeed = _initialMovementSpeed;
        }

        protected override void OnEnable()
        {
            SIEventsHandler.OnObjectMovement += MoveObj;
            SIEventsHandler.OnEnemySpeedMultiplierChanged += UpdateMovementStep;
            SIEventsHandler.OnWaveEnd += ResetEnemy;
            onScreenEdgeAction += TryToMoveObjectDown;
        }

        protected override void OnDisable()
        {
            SIEventsHandler.OnObjectMovement -= MoveObj;
            SIEventsHandler.OnEnemySpeedMultiplierChanged -= UpdateMovementStep;
            SIEventsHandler.OnWaveEnd -= ResetEnemy;

            onScreenEdgeAction -= TryToMoveObjectDown;
        }

        public void MoveObj()
        {
            if (SIEnemiesGridsMaster.Instance.IsEnemyInGridMovementAllowed == false)
            {
                return;
            }

            MoveObject(_moveStep, true);

        }

        private Vector3 TryToMoveObjectDown(Vector3 objectInCameraBoundsPos)
        {
            if (objectInCameraBoundsPos.IsObjectInScreenHorizontalBounds3D())
            { 
                objectInCameraBoundsPos.y -= VERTICAL_MOVEMENT_VIEWPORT_STEP;

                StopAllCoroutines();
                SetMovementDirectionProperties();
                StartCoroutine(RotateRoutine());
            }

            return objectInCameraBoundsPos;
        }

        private void SetMovementDirectionProperties()
        {
            _currentMovementSpeed = -_currentMovementSpeed;
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
            _moveStep = newStep;
        }

        private void ResetEnemy()
        {
            _cachedTransform.position = _startPosition;

            ResetMovementProperties();
        }

    }
}
