using UnityEditor;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIGameMasterBehaviour : SIGenericSingleton<SIGameMasterBehaviour>
    {
        [SerializeField] private bool _isGameStarted;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private SIPlayerBehaviour _player;

        public Camera MainCamera
        {
            get
            {
                if (_mainCamera == null)
                {
                    SIHelpers.SISimpleLogger(this, "No camera assigned!", SimpleLoggerTypes.Error);
                }

                return _mainCamera;
            }
        }

        public SIPlayerBehaviour Player
        {
            get
            {
                if (_player != null)
                {
                    return _player;
                }

                SIHelpers.SISimpleLogger(this, "No player assigned to SIGameMasterBehaviour", SimpleLoggerTypes.Error);
                return null;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            Application.targetFrameRate = SISettings.APPLICATION_TARGET_FRAMERATE;
        }

        private void OnEnable()
        {
            AssignEvents();
        }

        private void AssignEvents()
        {
            SIEventsHandler.OnGameStarted += StartGame;
            SIEventsHandler.OnGameQuit += QuitGame;
        }

        private void OnDisable()
        {
            RemoveEvents();
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnGameStarted -= StartGame;
            SIEventsHandler.OnGameQuit -= QuitGame;
        }

        private void StartGame()
        {
            _isGameStarted = true;
        }

        private void Update()
        {
            if (_isGameStarted == false)
            {
                return;
            }

            RunGameLoop();
        }

        private void RunGameLoop()
        {
            OnUpdateMovements();
            OnShadersUpdateCallback();
            OnDebugInputHandling();
            OnGameQuitCallback();
        }

        private void OnDebugInputHandling()
        {
            SIEventsHandler.BroadcastOnDebugInputHandling();
        }

        private void OnGameQuitCallback()
        {
            SIEventsHandler.BroadcastOnGameQuit();
        }

        private void OnShadersUpdateCallback()
        {
            SIEventsHandler.BroadcastOnShadersUpdate();
        }

        private void QuitGame()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                    Application.Quit();
#elif UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }

        private void OnUpdateMovements()
        {
            SIEventsHandler.BroadcastOnObjectsMovement();
        }

        public void OnGameStarted()
        {
            SIEventsHandler.BroadcastOnGameStarted();
        }
    }
}