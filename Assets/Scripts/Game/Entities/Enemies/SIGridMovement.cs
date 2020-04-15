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

            _canMove = true;
            _gridMovementSettings = _gridMovementSetup.gridMovementSettings;
            _worldScreenEdges = SIGameMasterBehaviour.Instance.ScreenAreaCalculator.CalculatedScreenEdges;
            _rightScreenEdgeOffset = _worldScreenEdges.rightScreenEdge - _screenEdgeOffset;
            _leftScreenEdgeOffset = _worldScreenEdges.leftScreenEdge + _screenEdgeOffset;
            _initialMovementSpeed = _gridMovementSettings.initialMovementSpeed;
            _currentMovementSpeed = _initialMovementSpeed;

            InitialiseTweeners();
            UpdateMovementOffsets();
        }

        void InitialiseTweeners()
        {
            _initialMovementTweener = _thisTransform
                .DOLocalMove(_gridMovementSettings.worldTargetPosition, _gridMovementSettings.initialMovementEaseDuration)
                .OnPlay(() => _isInitialSequenceFinished = false)
                .OnComplete(() => _isInitialSequenceFinished = true)
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
            SIEnemyGridEvents.OnGridStarted += HandleOnGridStarted;
            SIEnemyGridEvents.OnGridReset += HandleOnGridReset;
            SIEventsHandler.OnUpdate += HandleOnUpdate;
            SIEventsHandler.OnEnemyDeath += HandleOnEnemyDeath;
            SIEventsHandler.OnEnemySpeedMultiplierChanged += HandleOnEnemySpeedMultiplierChanged;
        }

        protected override void RemoveEvents()
        {
            SIEnemyGridEvents.OnGridStarted -= HandleOnGridStarted;
            SIEnemyGridEvents.OnGridReset -= HandleOnGridReset;
            SIEventsHandler.OnUpdate -= HandleOnUpdate;
            SIEventsHandler.OnEnemyDeath -= HandleOnEnemyDeath;
            SIEventsHandler.OnEnemySpeedMultiplierChanged -= HandleOnEnemySpeedMultiplierChanged;
        }

        void HandleOnGridStarted()
        {
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

        void HandleOnEnemySpeedMultiplierChanged(float multiplier)
        {
            TryToUpdateCurrentGridMovementSpeed(multiplier);
        }

        void HandleOnGridReset()
        {
            ResetGridMovement();
        }

        void ExecuteInitialMovementSequence()
        {
            _initialMovementTweener.Restart();
        }

        void TryToUpdateCurrentGridMovementSpeed(float multiplier)
        {
//            _currentSpeedMultiplier += multiplier;
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
            if (!_isInitialSequenceFinished  || _canMove == false)
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

        void ResetGridMovement()
        {
            _canMove = true;
            _currentMovementSpeed = Mathf.Abs(_currentMovementSpeed);
            _currentMovementSpeed += _gridMovementSettings.newWaveInitialSpeedChange;
            _currentMovementSpeed = Mathf.Clamp(_currentMovementSpeed, _initialMovementSpeed,
                _gridMovementSettings.maxMovementSpeed);
            _currentSpeedMultiplier = _gridMovementSettings.initialSpeedMultiplier;
            _initialMovementTweener.Pause();
            _verticalMovementTweener.Pause();
            UpdateMovementOffsets();
        }
    }
}