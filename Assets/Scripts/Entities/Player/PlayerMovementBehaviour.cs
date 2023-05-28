using PG.Game.Configs;
using PG.Game.EventSystem;
using PG.Game.Helpers;
using PG.Game.Systems;
using UnityEngine;

namespace PG.Game.Entities.Player {
    public class PlayerMovementBehaviour : MovementBehaviour {
        [SerializeField] Transform _moveableContent;
        [Range(0f, 3f)] [SerializeField] float _screenEdgeOffset;
        Quaternion _fromRotation;
        float _fromTimeMod;

        Vector3 _inputValue;
        float _leftScreenOffset;
        [SerializeField] PlayerMovementSettings _playerMovementSettings;
        [SerializeField] PlayerMovementSetup _playerMovementSetup;

        float _rightScreenOffset;
        Quaternion _toRotation;
        float _toTimeMod;
        ScreenEdges _worldScreenEdges;

        public override void SetTimeSpeedModifier(float timeSpeedModifier, float progress = 1) {
            //Player slows down when all are fast and opposite. Maybe I'll change this mechanic in some time.
            _speedModificator = Mathf.Lerp(_fromTimeMod, _toTimeMod, progress);
        }

        protected override void Initialise() {
            base.Initialise();
            _canMove = true;
            _playerMovementSettings = _playerMovementSetup.playerMovementSettings;
            _worldScreenEdges = GameMasterBehaviour.Instance.ScreenAreaCalculator.CalculatedScreenEdges;
            _rightScreenOffset = _worldScreenEdges.rightScreenEdge - _screenEdgeOffset;
            _leftScreenOffset = _worldScreenEdges.leftScreenEdge + _screenEdgeOffset;
            _initialMovementSpeed = _playerMovementSettings.initialMovementSpeed;
            _currentMovementSpeed = _initialMovementSpeed;
            SetSpeedModificatorFromBonus(_playerMovementSettings.defaultTimeModMultiplier);

            if (_moveableContent == null)
                Debug.LogError($"{nameof(PlayerMovementBehaviour)} No moveable content attached!");
            else
                _thisTransform = _moveableContent;
            SetTimeSpeedModifier(_playerMovementSettings.defaultTimeModMultiplier);
        }

        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            GeneralEvents.OnAxesInputReceived += HandleAxesInputReceived;
            BonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            BonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
        }

        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            GeneralEvents.OnAxesInputReceived -= HandleAxesInputReceived;
            BonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            BonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
        }

        protected override bool IsMovementPossible() {
            return _canMove;
        }

        void HandleOnBonusEnabled(BonusSettings bonusSettings) {
            switch (bonusSettings.bonusType) {
                case BonusType.TimeModSlowAll:
                    SetSpeedModificatorFromBonus(_playerMovementSettings.timeModSlowAllMultiplier);
                    break;
                case BonusType.TimeModeFastAll:
                    SetSpeedModificatorFromBonus(_playerMovementSettings.timeModFastAllMultiplier);
                    break;
            }
        }

        void HandleOnBonusDisabled(BonusSettings bonusSettings) {
            if (bonusSettings.bonusType == BonusType.TimeModSlowAll || bonusSettings.bonusType == BonusType.TimeModeFastAll)
                SetSpeedModificatorFromBonus(_playerMovementSettings.defaultTimeModMultiplier);
        }

        void HandleAxesInputReceived(Vector3 inputVector) {
            _inputValue = inputVector;
        }

        void SetSpeedModificatorFromBonus(float modificator) {
            _fromTimeMod = _playerMovementSettings.defaultTimeModMultiplier;
            _toTimeMod = modificator;
        }

        /*
         * Implement SetMovementPossibility when player dies.
         */

        protected override void UpdatePosition() {
            _dt = Time.deltaTime;
            float horizontalMovementDelta = _dt * _inputValue.x * _currentMovementSpeed * _speedModificator;
            float verticalMovementDelta = _dt * _inputValue.y * _currentMovementSpeed * _speedModificator;

            Vector3 currentPosition = _thisTransform.position;
            Vector3 newPosition = new(currentPosition.x + horizontalMovementDelta,
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

        protected override void UpdateRotation() {
            _fromRotation = _thisTransform.rotation;
            _toRotation = Quaternion.Euler(0, -_inputValue.x * _playerMovementSettings.maxRotationAngle, 0);
            _thisTransform.rotation =
                Quaternion.Slerp(_fromRotation, _toRotation, _playerMovementSettings.movementSmoothStep);
        }
    }
}