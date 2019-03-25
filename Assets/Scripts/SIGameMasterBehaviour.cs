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
                    Debug.LogError("No camera assigned!");
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
            SIEventsHandler.OnGameStarted += StartGame;
            SIEventsHandler.OnGameQuit += QuitGame;
        }

        private void OnDisable()
        {
            SIEventsHandler.OnGameStarted -= StartGame;
            SIEventsHandler.OnGameQuit -= QuitGame;
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
                Application.Quit();
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

        private void StartGame()
        {
            _isGameStarted = true;
        }
    }
}
