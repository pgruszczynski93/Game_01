using DG.Tweening;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIGridMovement : SIMovement
    {
        [SerializeField] GridMovementSetup _gridMovementSetup;

        [SerializeField] float _currentSpeedMultiplier;
        [Range(0f, 3f), SerializeField] protected float _screenEdgeOffset;
        [SerializeField] SIGridMovementLimiter gridMovementLimiter;

        bool _isInHorizontalLimits;
        bool _isTweeningVerticalMovement;
        bool _isInitialSequenceFinished;
        int _gridMovementSpeedTier;
        float _rightScreenEdgeOffset;
        float _leftScreenEdgeOffset;

        LocalGridMinMax _gridMinMax;
        ScreenEdges _worldScreenEdges;
        GridMovementSettings _gridMovementSettings;
        Tweener _initialMovementTweener;
        Tweener _verticalMovementTweener;

        protected override void Initialise()
        {
            base.Initialise();

            _gridMovementSpeedTier = 0;
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
                .DOLocalMove(_gridMovementSettings.worldTargetPosition, _gridMovementSettings.initialMovementEaseDuration)
                .OnPlay(() => _isInitialSequenceFinished = false)
                .OnComplete(() =>
                {
                    _isInitialSequenceFinished = true;
                    _canMove = true;
                    SIEnemyGridEvents.BroadcastOnGridOnGridShootingReset();
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

        protected override void AssignEvents()
        {
            SIEnemyGridEvents.OnGridReset += HandleOnGridReset;
            SIEventsHandler.OnUpdate += HandleOnUpdate;
            SIEventsHandler.OnEnemyDeath += HandleOnEnemyDeath;
            SIEnemyGridEvents.OnUpdateGridMovementSpeedTier += HandleOnEnemySpeedMultiplierChanged;
        }

        protected override void RemoveEvents()
        {
            SIEnemyGridEvents.OnGridReset -= HandleOnGridReset;
            SIEventsHandler.OnUpdate -= HandleOnUpdate;
            SIEventsHandler.OnEnemyDeath -= HandleOnEnemyDeath;
            SIEnemyGridEvents.OnUpdateGridMovementSpeedTier -= HandleOnEnemySpeedMultiplierChanged;
        }

        void HandleOnGridReset()
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

        void HandleOnEnemySpeedMultiplierChanged(int tier)
        {
            TryToUpdateCurrentMovementSpeed(_gridMovementSettings.gridMovementSpeedTiers[tier]);
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
        void TryToUpdateCurrentMovementSpeed(float multiplier)
        {
            _currentSpeedMultiplier = multiplier;
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
            if (!_isInitialSequenceFinished  || !_canMove)
                return;
            UpdatePosition();
        }

        protected override void TryToStopObject()
        {
            _canMove = false;
        }

        protected override void UpdatePosition()
        {
            _dt = Time.deltaTime;

            Vector3 currentPosition = _thisTransform.position;
            float horizontalMovementDelta = _dt * _currentSpeedMultiplier * _currentMovementSpeed;
            _isInHorizontalLimits =
                SIScreenUtils.IsInHorizontalWorldScreenLimit(currentPosition, _leftScreenEdgeOffset,
                    _rightScreenEdgeOffset);

            if (_isInHorizontalLimits)
            {
                MoveObjectHorizontally(currentPosition, horizontalMovementDelta);
            }
            else
            {
                ClampAndInverseObjectMovementToScreenBounds(currentPosition);
            }
        }

        void MoveObjectHorizontally(Vector3 currentPosition, float horizontalMovementDelta)
        {
            Vector3 newPosition = new Vector3(currentPosition.x + horizontalMovementDelta,
                currentPosition.y, 0f);
            Vector3 smoothedPosition =
                Vector3.Lerp(currentPosition, newPosition, _gridMovementSettings.movementSmoothStep);
            _thisTransform.position = smoothedPosition;
        }

        void ClampAndInverseObjectMovementToScreenBounds(Vector3 currentPosition)
        {
            if (!_isTweeningVerticalMovement)
                MoveObjectVertically(currentPosition);
        }

        void MoveObjectVertically(Vector3 currentPosition)
        {
            float clampedHorizontalPos =
                Mathf.Clamp(currentPosition.x, _leftScreenEdgeOffset, _rightScreenEdgeOffset);

            Vector3 verticalTargetPositon = new Vector3(
                clampedHorizontalPos,
                currentPosition.y - _gridMovementSettings.gridDownStep,
                currentPosition.z);

            _verticalMovementTweener.Pause()
                .ChangeEndValue(verticalTargetPositon, true)
                .Restart();
        }
        
        protected override void UpdateRotation()
        {
            //Intentionally not implemented.
        }
        
    }
}