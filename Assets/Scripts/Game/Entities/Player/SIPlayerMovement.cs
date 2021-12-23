using DG.Tweening;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerMovement : SIMovement
    {
        [SerializeField] PlayerMovementSetup _playerMovementSetup;
        [SerializeField] PlayerMovementSettings _playerMovementSettings;

        [Range(0f, 3f), SerializeField] protected float _screenEdgeOffset;

        float _rightScreenOffset;
        float _leftScreenOffset;
        Vector3 _inputValue;
        ScreenEdges _worldScreenEdges;
        Quaternion _fromRotation;
        Quaternion _toRotation;

        protected override void Initialise()
        {
            base.Initialise();

            _canMove = true;
            _playerMovementSettings = _playerMovementSetup.playerMovementSettings;
            _worldScreenEdges = SIGameMasterBehaviour.Instance.ScreenAreaCalculator.CalculatedScreenEdges;
            _rightScreenOffset = _worldScreenEdges.rightScreenEdge - _screenEdgeOffset;
            _leftScreenOffset = _worldScreenEdges.leftScreenEdge + _screenEdgeOffset;
            _initialMovementSpeed = _playerMovementSettings.initialMovementSpeed;
            _currentMovementSpeed = _initialMovementSpeed;
            SetTimeSpeedModifier(_playerMovementSettings.defaultSpeedModificator);
        }

        protected override void SubscribeEvents()
        {
            SIEventsHandler.OnUpdate += TryToMoveObject;
            SIEventsHandler.OnAxesInputReceived += HandleAxesInputReceived;
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
        }

        protected override void UnsubscribeEvents()
        {
            SIEventsHandler.OnUpdate -= TryToMoveObject;
            SIEventsHandler.OnAxesInputReceived += HandleAxesInputReceived;
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
        }

        void HandleOnBonusEnabled(BonusSettings bonusSettings) {
            if (bonusSettings.bonusType == BonusType.TimeModification) {
                SetTimeSpeedModifier(_playerMovementSettings.slowDownBonusSpeedModificator);
            }
        }
        
        void HandleOnBonusDisabled(BonusSettings bonusSettings) {
            if (bonusSettings.bonusType == BonusType.TimeModification) {
                SetTimeSpeedModifier(_playerMovementSettings.defaultSpeedModificator);
            }
        }

        void HandleAxesInputReceived(Vector3 inputVector)
        {
            _inputValue = inputVector;
        }

        protected override void TryToMoveObject()
        {
            if (_canMove == false)
                return;

            UpdatePosition();
            UpdateRotation();
        }

        protected override void TryToStopObject()
        {
            //todo: stop conditiions

            if (_canMove)
                return;

            _canMove = false;
        }
        
        protected override void UpdatePosition()
        {
            _dt = Time.deltaTime;
            float horizontalMovementDelta = _dt * _inputValue.x * _currentMovementSpeed * _speedModificator;
            float verticalMovementDelta = _dt * _inputValue.y * _currentMovementSpeed * _speedModificator;

            Vector3 currentPosition = _thisTransform.position;
            Vector3 newPosition = new Vector3(currentPosition.x + horizontalMovementDelta,
                currentPosition.y /* + verticalMovementDelta*/, 0f);
            Vector3 smoothedPosition =
                Vector3.Lerp(currentPosition, newPosition, _playerMovementSettings.movementSmoothStep);

            float clampedHorizontalPos =
                Mathf.Clamp(smoothedPosition.x, _leftScreenOffset, _rightScreenOffset);

            //NOTE: for now vertical movement is locked - remove it when necessary and tweak.

//            float clampedVerticalPos = Mathf.Clamp(smoothedPosition.y, _screenEdges.bottomScreenEdge,
//                _screenEdges.topScreenEdge);
            float clampedVerticalPos = smoothedPosition.y;

            smoothedPosition = new Vector3(clampedHorizontalPos, clampedVerticalPos, smoothedPosition.z);
            _thisTransform.position = smoothedPosition;
        }

        protected override void UpdateRotation()
        {
            _fromRotation = _thisTransform.rotation;
            _toRotation = Quaternion.Euler(0, -_inputValue.x * _playerMovementSettings.maxRotationAngle, 0);
            _thisTransform.rotation =
                Quaternion.Slerp(_fromRotation, _toRotation, _playerMovementSettings.movementSmoothStep);
        }
    }
}