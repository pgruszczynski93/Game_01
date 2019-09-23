using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyMovement : SIMovement, ICanMove
    {
        [SerializeField] private float _horizontalMovementMultiplier;
        [SerializeField] private float _verticalMovementDownstep;

        //todo: przerobić uzywajac napisanego smoothutilsa
//        [SerializeField] private QuaternionTweenInfo _tweenInfo;

        private int _rotationDirection;
        private float _speedIncrementValue;
        private float _currentMovementMultiplier;
        private Quaternion _enemyObjectOrientation;

        protected override void Initialise()
        {
            base.Initialise();

            ResetMovement();
        }

        protected override void ResetMovement()
        {
            base.ResetMovement();

            _maxRotationAngle = 20;
            _horizontalMovementMultiplier = 1.5f;
            _currentMovementMultiplier = _horizontalMovementMultiplier;
            _speedIncrementValue = 0.2f;
            _rotationDirection = 1;
            _enemyObjectOrientation = Quaternion.Euler(90, 0, 180);
            _cachedTransform.localRotation = _enemyObjectOrientation;
            _currentMovementSpeed = _initialMovementSpeed;
            _canMove = true;
        }

        protected override void OnEnable()
        {
//            SIEventsHandler.OnUpdate += MoveObject;
            SIEventsHandler.OnEnemySpeedMultiplierChanged += UpdateMovementStep;
            SIEventsHandler.OnWaveEnd += ResetEnemy;
        }

        protected override void OnDisable()
        {
//            SIEventsHandler.OnUpdate -= MoveObject;
            SIEventsHandler.OnEnemySpeedMultiplierChanged -= UpdateMovementStep;
            SIEventsHandler.OnWaveEnd -= ResetEnemy;
        }

        public void MoveObject()
        {
            if (SIEnemiesGridsMaster.Instance.IsEnemyInGridMovementAllowed == false || _canMove == false)
                return;

            UpdatePosition();
            UpdateRotation();
        }

        protected override void UpdatePosition()
        {
            _dt = Time.deltaTime;

            Vector3 currentPosition = _cachedTransform.position;

            float horizontalMovementDelta = _dt * _currentMovementMultiplier * _currentMovementSpeed;
            bool isInScreenBounds = IsInHorizontalScreenBounds(currentPosition);

            if (isInScreenBounds)
            {
                Vector3 newPosition = new Vector3(currentPosition.x + horizontalMovementDelta,
                    currentPosition.y);
                Vector3 smoothedPosition = Vector3.Lerp(currentPosition, newPosition, _smoothMovementStep);
                _cachedTransform.position = smoothedPosition;
            }
            else
            {
                float clampedHorizontalPos =
                    Mathf.Clamp(currentPosition.x, _leftScreenOffset, _rightScreenOffset);

                _cachedTransform.position = new Vector3(
                    clampedHorizontalPos,
                    currentPosition.y - _verticalMovementDownstep,
                    currentPosition.z);


                UpdateMovementProperties();
            }
            
            //todo: a lot of tests, update grid accordingly to the speed, next refactor iteration
            //todo: now there's no acceleration each downstep
        }

        protected override void UpdateRotation()
        {
        }

        private bool IsInHorizontalScreenBounds(Vector3 currentPosition)
        {
            float horizontalPosition = currentPosition.x;
            return horizontalPosition >= _leftScreenOffset && horizontalPosition <= _rightScreenOffset;
        }

        private void UpdateMovementProperties()
        {
            _currentMovementSpeed = -_currentMovementSpeed;
//            _currentMovementMultiplier += _speedIncrementValue;
            _rotationDirection = (_currentMovementSpeed > 0) ? 1 : -1;
        }

        private void UpdateMovementStep(float newStep)
        {
            _currentMovementMultiplier = newStep;
        }

        private void ResetEnemy()
        {
            _cachedTransform.localPosition = _startPosition;
            _currentMovementMultiplier = _horizontalMovementMultiplier;

            ResetMovement();
        }

        public void StopObject()
        {
            _canMove = false;
        }

//        private void SetTweenInfoValues(float zAngle = 0.0f)
//        {
//            _tweenInfo.startValue = _cachedTransform.rotation;
//            _tweenInfo.endValue = Quaternion.Euler(90, 0, 180 + zAngle);
//        }

//
//        private IEnumerator RotateRoutine()
//        {
//            SetTweenInfoValues(_rotationDirection * _maxRotationAngle);
//
//            yield return StartCoroutine(SIHelpers.SimpleTween3D((outQuaternion) =>
//                {
//                    _cachedTransform.rotation = outQuaternion;
//                }, _tweenInfo, null));
//
//        }
    }
}