using PG.Game.Entities;
using PG.Game.Entities.Player;
using PG.Game.EventSystem;
using PG.Game.Helpers;
using PG.Game.Systems;
using UnityEngine;

namespace PG.Game.Asteroids {
    public class AsteroidBehaviour : MonoBehaviour, ICanMove {
        [SerializeField] float _minForce;
        [SerializeField] float _maxForce;

        [SerializeField] AsteroidState _asteroidState;
        [SerializeField] Rigidbody _rigidbody;
        [SerializeField] Renderer _renderer;

        bool _initialised;
        bool _isMoving;
        Transform _thisTransform;
        Vector3 _startPosition;
        Camera _mainCamera;
        PlayerBehaviour _player;

        void Initialise() {
            if (_initialised)
                return;

            _initialised = true;
            _thisTransform = transform;
            _startPosition = _thisTransform.localPosition;
            _mainCamera = GameMasterBehaviour.Instance.MainCamera;
            _player = GameMasterBehaviour.Instance.Player;
            _asteroidState = AsteroidState.ReadyToMove;
            RotateTowardsScreen();
        }

        void Start() => Initialise();
        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();

        void SubscribeEvents() {
            GeneralEvents.OnUpdate += CheckIsObjectVisibleOnScreen;
        }

        void UnsubscribeEvents() {
            GeneralEvents.OnUpdate -= CheckIsObjectVisibleOnScreen;
        }

        public void MoveObject() {
            if (_isMoving)
                return;

            float forceMultiplier = Random.Range(_minForce, _maxForce);
            Vector3 forward = _thisTransform.forward;
            Vector3 multipliedMoveVector = forward * forceMultiplier;
            _rigidbody.AddForce(multipliedMoveVector, ForceMode.Impulse);
            _rigidbody.AddTorque(multipliedMoveVector, ForceMode.Impulse);
            _isMoving = true;
        }

        void ResetMovement() {
            _isMoving = false;
            _rigidbody.velocity = MathConsts.VectorZero;
            _rigidbody.angularVelocity = MathConsts.VectorZero;
            _asteroidState = AsteroidState.ReadyToMove;
            _thisTransform.localPosition = _startPosition;
            RotateTowardsScreen();
        }


        void RotateTowardsScreen() {
            Vector3 toPlayerDirection = (_player.transform.localPosition - _startPosition).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(toPlayerDirection, Vector3.up);
            _thisTransform.localRotation = lookRotation;
            Vector3 localEulerAngles = _thisTransform.localEulerAngles;
            Vector3 eulerRotation =
                new Vector3(localEulerAngles.x, ConvertEulerY(localEulerAngles.y), localEulerAngles.z);
            _thisTransform.localRotation = Quaternion.Euler(eulerRotation);
        }

        float ConvertEulerY(float localEulerY) {
            if (localEulerY >= 0.0f && localEulerY <= 180.0f)
                return 90.0f;
            return 270.0f;
        }

        public void StopObject() {
            ResetMovement();
        }

        void CheckIsObjectVisibleOnScreen() {
            bool isObjectVisible = ScreenUtils.IsInCameraFrustum(_renderer, _mainCamera);

            if (isObjectVisible == false && _asteroidState == AsteroidState.OnScreen) {
                StopObject();
                return;
            }

            if (isObjectVisible)
                _asteroidState = AsteroidState.OnScreen;
        }
    }
}