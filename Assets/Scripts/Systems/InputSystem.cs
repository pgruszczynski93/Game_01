using PG.Game.EventSystem;
using PG.Game.Helpers;
using UnityEngine;

namespace PG.Game.Systems {
    public class InputSystem : MonoBehaviour {
        [SerializeField] Joystick _joystick;

        float _horizontalAxis;
        float _verticalAxis;
        float _depthAxis;
        Vector3 _inputVector;

        void Initialise() {
            _depthAxis = 0f;
        }

        void Start() => Initialise();
        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();

        void SubscribeEvents() {
            GeneralEvents.OnUpdate += HandleOnUpdate;
        }

        void UnsubscribeEvents() {
            GeneralEvents.OnUpdate -= HandleOnUpdate;
        }

        void HandleOnUpdate() {
            GetInputAxes();
            // TryToSendShootAction();
            TryToQuitGame();
        }

        public void TryToStartGame() {
            //Todo: change it when GUI will be implemented.
            GeneralEvents.BroadcastOnGameStateChanged(GameStates.GameStarted);
        }

        // void TryToSendShootAction()
        // {
        //     if(Input.GetKeyDown(KeyCode.Space))
        //         SIGameplayEvents.BroadcastOnPlayerShoot();
        // }

        void TryToQuitGame() {
            if (!Input.GetKeyDown(KeyCode.Escape))
                return;

            GeneralEvents.BroadcastOnGameStateChanged(GameStates.GameQuit);
        }

        void GetInputAxes() {
#if UNITY_EDITOR

            _horizontalAxis = Input.GetAxis("Horizontal");
            _verticalAxis = Input.GetAxis("Vertical");

#elif UNITY_ANDROID && !UNITY_EDITOR
            _horizontalAxis = _joystick.Horizontal;
            _verticalAxis = _joystick.Vertical;

#endif
            _inputVector = new Vector3(_horizontalAxis, _verticalAxis, _depthAxis);
            GeneralEvents.BroadcastOnAxesInputReceived(_inputVector);
        }

        public void TestShoot() {
            GameplayEvents.BroadcastOnPlayerShoot();
        }
    }
}