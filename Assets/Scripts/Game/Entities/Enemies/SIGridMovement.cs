using DG.Tweening;
using UnityEngine;

namespace SpaceInvaders {
    public class SIGridMovement : SIMovement {
        [SerializeField] GridMovementSetup _gridMovementSetup;
        [SerializeField] float _currentSpeedMultiplier;
        [Range(0f, 3f), SerializeField] protected float _screenEdgeOffset;
        [SerializeField] SIGridMovementLimiter gridMovementLimiter;

        bool _isTweeningVerticalMovement;
        bool _isInitialSequenceFinished;
        bool _gridBecameVisible;
        int _gridMovementSpeedTier;
        float _rightScreenEdgeOffset;
        float _leftScreenEdgeOffset;
        float _nextSpeedMultiplier;

        LocalGridMinMax _gridMinMax;
        ScreenEdges _worldScreenEdges;
        GridMovementSettings _gridMovementSettings;
        Tweener _initialMovementTweener;
        Tweener _verticalMovementTweener;

        protected override void Initialise() {
            base.Initialise();

            _gridMovementSettings = _gridMovementSetup.gridMovementSettings;
            _worldScreenEdges = SIGameMasterBehaviour.Instance.ScreenAreaCalculator.CalculatedScreenEdges;
            _rightScreenEdgeOffset = _worldScreenEdges.rightScreenEdge - _screenEdgeOffset;
            _leftScreenEdgeOffset = _worldScreenEdges.leftScreenEdge + _screenEdgeOffset;
            _initialMovementSpeed = _gridMovementSettings.initialMovementSpeed;
            _currentMovementSpeed = _initialMovementSpeed;
            SIGameplayEvents.BroadcastOnSpeedModificationRequested(this);
            InitialiseTweeners();
        }

        void InitialiseTweeners() {
            _initialMovementTweener = _thisTransform
                .DOLocalMove(_gridMovementSettings.worldTargetPosition,
                    _gridMovementSettings.initialMovementEaseDuration)
                .OnPlay(() => _isInitialSequenceFinished = false)
                .OnComplete(() => {
                    _isInitialSequenceFinished = true;
                    _canMove = true;
                    _isMoving = true;
                })
                .SetEase(_gridMovementSettings.initialMovementEaseType)
                .SetAutoKill(false)
                .Pause();

            _verticalMovementTweener = _thisTransform
                .DOMove(_gridMovementSettings.worldTargetPosition, _gridMovementSettings.horizontalDownstepDuration)
                .OnPlay(() => _isTweeningVerticalMovement = true)
                .OnComplete(() => {
                    _isTweeningVerticalMovement = false;
                    _currentMovementSpeed = -_currentMovementSpeed;
                })
                .SetEase(_gridMovementSettings.horizontalDownstepEaseType)
                .SetAutoKill(false)
                .Pause();
        }

        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            SIGameplayEvents.OnWaveStart += HandleOnWaveStart;
            SIGameplayEvents.OnWaveEnd += HandleOnWaveEnd;
            SIGameplayEvents.OnEnemyDeath += HandleOnEnemyDeath;
        }

        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            SIGameplayEvents.OnWaveStart -= HandleOnWaveStart;
            SIGameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
            SIGameplayEvents.OnEnemyDeath -= HandleOnEnemyDeath;
        }

        void HandleOnWaveStart() {
            ExecuteInitialMovementSequence();
        }

        void HandleOnWaveEnd() {
            //=== Don't remove ===
            //This method ensures that movement limits will be recalculated with each wave.
            ResetGridMovement();
            UpdateMovementOffsets();
        }

        protected override void HandleOnUpdate() {
            CheckInitialVisibility();

            base.HandleOnUpdate();
        }

        void HandleOnEnemyDeath(SIEnemyBehaviour enemyBehaviours) {
            UpdateMovementOffsets();
        }

        void ExecuteInitialMovementSequence() {
            _initialMovementTweener.Restart();
        }

        void CheckInitialVisibility() {
            Vector3 bonusViewPortPosition =
                SIGameMasterBehaviour.Instance.MainCamera.WorldToViewportPoint(_thisTransform.position);
            if (!bonusViewPortPosition.IsInVerticalViewportSpace() || _gridBecameVisible)
                return;
            _gridBecameVisible = true;
            SIEnemyGridEvents.BroadcastOnGridOnGridVisibilityChanged(true);
        }

        void ResetGridMovement() {
            Debug.Log($"[SIGridMovement] ResetGrid movement()");
            TryToStopObject();
            _currentMovementSpeed = Mathf.Abs(_currentMovementSpeed);
//            _currentMovementSpeed += _gridMovementSettings.newWaveInitialSpeedChange;
//            _currentMovementSpeed = Mathf.Clamp(_currentMovementSpeed, _initialMovementSpeed,
//                _gridMovementSettings.maxMovementSpeed);
            _currentSpeedMultiplier = _gridMovementSettings.initialSpeedMultiplier;
            _initialMovementTweener.Pause();
            _verticalMovementTweener.Pause();

            _gridBecameVisible = false;
            _thisTransform.position = _gridMovementSettings.worldStartPosition;
            SIEnemyGridEvents.BroadcastOnGridOnGridVisibilityChanged(false);
        }

        public void UpdateCurrentMovementSpeed() {
            _nextSpeedMultiplier = _currentSpeedMultiplier + _gridMovementSettings.gridMovementSpeedUpStep;
            _currentSpeedMultiplier = _nextSpeedMultiplier;
        }

        void UpdateMovementOffsets() {
            _gridMinMax = gridMovementLimiter.CalculateGridMinMax();
            _rightScreenEdgeOffset = _worldScreenEdges.rightScreenEdge - _gridMinMax.localGridHorizontalMax -
                                     _gridMovementSettings.enemyWidthOffset;
            _leftScreenEdgeOffset = _worldScreenEdges.leftScreenEdge - _gridMinMax.localGridHorizontalMin +
                                    _gridMovementSettings.enemyWidthOffset;
        }

        protected override bool IsMovementPossible() {
            return _isInitialSequenceFinished && _canMove;
        }

        protected override void TryToStopObject() {
            _canMove = false;
            _isMoving = true;
            _isTweeningVerticalMovement = false;
        }

        protected override void UpdatePosition() {
            _dt = Time.deltaTime;

            Vector3 currentPosition = _thisTransform.position;
            float posDelta = _dt * _currentSpeedMultiplier * _currentMovementSpeed * _speedModificator;
            Vector3 nextPosition =
                new Vector3(currentPosition.x + posDelta, currentPosition.y, currentPosition.z);

            if (_isTweeningVerticalMovement)
                return;

            if (IsInHorizontalMovementRange(nextPosition.x, _leftScreenEdgeOffset, _rightScreenEdgeOffset))
                MoveObjectHorizontally(currentPosition, nextPosition);
            else
                MoveObjectVertically(nextPosition);
        }

        protected override void UpdateRotation() {
            //Intentionally not implemented.
        }

        bool IsInHorizontalMovementRange(float nextPosX, float leftLimit, float rightLimit) {
            return nextPosX >= leftLimit && nextPosX <= rightLimit;
        }

        void MoveObjectHorizontally(Vector3 currentPosition, Vector3 targetPosition) {
            Vector3 smoothedPosition =
                Vector3.Lerp(currentPosition, targetPosition, _gridMovementSettings.movementSmoothStep);
            _thisTransform.position = smoothedPosition;
        }

        void MoveObjectVertically(Vector3 newPosition) {
            Vector3 startPos = new Vector3(
                Mathf.Clamp(newPosition.x, _leftScreenEdgeOffset, _rightScreenEdgeOffset),
                newPosition.y, newPosition.z);

            Vector3 targetPos = new Vector3(startPos.x,
                startPos.y - _gridMovementSettings.gridDownStep,
                startPos.z);

            _verticalMovementTweener.Pause()
                .ChangeStartValue(startPos)
                .ChangeEndValue(targetPos, true)
                .Restart();
        }
    }
}