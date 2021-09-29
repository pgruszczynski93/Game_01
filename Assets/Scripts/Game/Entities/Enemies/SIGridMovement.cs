using DG.Tweening;
using UnityEngine;

namespace SpaceInvaders {
    public class SIGridMovement : SIMovement {
        [SerializeField] GridMovementSetup _gridMovementSetup;
        [SerializeField] float _currentSpeedMultiplier;
        [Range(0f, 3f), SerializeField] protected float _screenEdgeOffset;
        [SerializeField] SIGridMovementLimiter gridMovementLimiter;

        bool _isNextStepPossible;
        bool _isTweeningVerticalMovement;
        bool _isInitialSequenceFinished;
        int _gridMovementSpeedTier;
        float _rightScreenEdgeOffset;
        float _leftScreenEdgeOffset;
        float _nextSpeedMultiplier;

        LocalGridMinMax _gridMinMax;
        ScreenEdges _worldScreenEdges;
        GridMovementSettings _gridMovementSettings;
        Tweener _initialMovementTweener;
        Tweener _verticalMovementTweener;

        protected override void Initialise()
        {
            base.Initialise();

            _gridMovementSettings = _gridMovementSetup.gridMovementSettings;
            _worldScreenEdges = SIGameMasterBehaviour.Instance.ScreenAreaCalculator.CalculatedScreenEdges;
            _rightScreenEdgeOffset = _worldScreenEdges.rightScreenEdge - _screenEdgeOffset;
            _leftScreenEdgeOffset = _worldScreenEdges.leftScreenEdge + _screenEdgeOffset;
            _initialMovementSpeed = _gridMovementSettings.initialMovementSpeed;
            _currentMovementSpeed = _initialMovementSpeed;

            InitialiseTweeners();
        }

        void InitialiseTweeners()
        {
            _initialMovementTweener = _thisTransform
                .DOLocalMove(_gridMovementSettings.worldTargetPosition,
                    _gridMovementSettings.initialMovementEaseDuration)
                .OnPlay(() => _isInitialSequenceFinished = false)
                .OnComplete(() =>
                {
                    _isInitialSequenceFinished = true;
                    _canMove = true;
                    SIGameplayEvents.BroadcastOnWaveStart();
                })
                .SetEase(_gridMovementSettings.initialMovementEaseType)
                .SetAutoKill(false)
                .Pause();

            _verticalMovementTweener = _thisTransform
                .DOMove(_gridMovementSettings.worldTargetPosition, _gridMovementSettings.horizontalDownstepDuration)
                .OnPlay(() => _isTweeningVerticalMovement = true)
                .OnComplete(() =>
                {
                    _isTweeningVerticalMovement = false;
                    _currentMovementSpeed = -_currentMovementSpeed;
                })
                .SetEase(_gridMovementSettings.horizontalDownstepEaseType)
                .SetAutoKill(false)
                .Pause();
        }

        protected override void SubscribeEvents()
        {
            SIGameplayEvents.OnWaveEnd += HandleOnWaveEnd;
            SIEventsHandler.OnUpdate += HandleOnUpdate;
            SIGameplayEvents.OnEnemyDeath += HandleOnEnemyDeath;
            SIEnemyGridEvents.OnUpdateGridMovementSpeedTier += HandleOnEnemySpeedMultiplierChanged;
        }

        protected override void UnsubscribeEvents()
        {
            SIGameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
            SIEventsHandler.OnUpdate -= HandleOnUpdate;
            SIGameplayEvents.OnEnemyDeath -= HandleOnEnemyDeath;
            SIEnemyGridEvents.OnUpdateGridMovementSpeedTier -= HandleOnEnemySpeedMultiplierChanged;
        }

        void HandleOnWaveEnd()
        {
            //TODO: === Don't remove ===: this method ensures that movement limits will be recalculated with each wave.
            ResetGridMovement();
            UpdateMovementOffsets();
            ExecuteInitialMovementSequence();
        }

        void HandleOnUpdate()
        {
            TryToMoveObject();
        }

        void HandleOnEnemyDeath(SIEnemyBehaviour enemyBehaviours)
        {
            UpdateMovementOffsets();
        }

        void HandleOnEnemySpeedMultiplierChanged()
        {
            TryToUpdateCurrentMovementSpeed();
        }

        void ExecuteInitialMovementSequence()
        {
            _initialMovementTweener.Restart();
        }

        void ResetGridMovement()
        {
            Debug.Log($"[SIGridMovement] ResetGrid movement()");
            TryToStopObject();
            _currentMovementSpeed = Mathf.Abs(_currentMovementSpeed);
//            _currentMovementSpeed += _gridMovementSettings.newWaveInitialSpeedChange;
//            _currentMovementSpeed = Mathf.Clamp(_currentMovementSpeed, _initialMovementSpeed,
//                _gridMovementSettings.maxMovementSpeed);
            _currentSpeedMultiplier = _gridMovementSettings.initialSpeedMultiplier;
            _initialMovementTweener.Pause();
            _verticalMovementTweener.Pause();
        }

        void TryToUpdateCurrentMovementSpeed() {
            _nextSpeedMultiplier = _currentSpeedMultiplier + _gridMovementSettings.gridMovementSpeedUpStep;
            _currentSpeedMultiplier = _nextSpeedMultiplier;
        }

        void UpdateMovementOffsets()
        {
            _gridMinMax = gridMovementLimiter.CalculateGridMinMax();
            _rightScreenEdgeOffset = _worldScreenEdges.rightScreenEdge - _gridMinMax.localGridHorizontalMax -
                                     _gridMovementSettings.enemyWidthOffset;
            _leftScreenEdgeOffset = _worldScreenEdges.leftScreenEdge - _gridMinMax.localGridHorizontalMin +
                                    _gridMovementSettings.enemyWidthOffset;
        }

        protected override void TryToMoveObject()
        {
            if (!_isInitialSequenceFinished || !_canMove)
                return;
            UpdatePosition();
        }

        protected override void TryToStopObject()
        {
            _canMove = false;
            _isTweeningVerticalMovement = false;
        }

        protected override void UpdatePosition()
        {
            _dt = Time.deltaTime;

            Vector3 currentPosition = _thisTransform.position;
            float posDelta = _dt * _currentSpeedMultiplier * _currentMovementSpeed;
            Vector3 nextPosition =
                new Vector3(currentPosition.x + posDelta, currentPosition.y, currentPosition.z);

            _isNextStepPossible = SIScreenUtils.IsInHorizontalWorldScreenLimit(nextPosition,
                _leftScreenEdgeOffset,
                _rightScreenEdgeOffset);

            if (_isTweeningVerticalMovement)
                return;

            if (_isNextStepPossible)
                MoveObjectHorizontally(currentPosition, nextPosition);
            else
                MoveObjectVertically(nextPosition);
        }

        void MoveObjectHorizontally(Vector3 currentPosition, Vector3 targetPosition)
        {
            Vector3 smoothedPosition =
                Vector3.Lerp(currentPosition, targetPosition, _gridMovementSettings.movementSmoothStep);
            _thisTransform.position = smoothedPosition;
        }

        void MoveObjectVertically(Vector3 newPosition)
        {
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

        protected override void UpdateRotation()
        {
            //Intentionally not implemented.
        }
    }
}