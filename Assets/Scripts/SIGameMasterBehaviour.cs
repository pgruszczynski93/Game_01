using UnityEngine;

namespace SpaceInvaders
{
    public class SIGameMasterBehaviour : SIGenericSingleton<SIGameMasterBehaviour>
    {
        [SerializeField] private bool _isGameStarted;
        [SerializeField] private Camera _mainCamera;

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

            OnUpdateMovements();
            OnDebugInputHandling();
            OnGameQuitCallback();
        }

        private void OnDebugInputHandling()
        {
            SIEventsHandler.OnDebugInputHandling?.Invoke();
        }

        private void OnGameQuitCallback()
        {
            SIEventsHandler.OnGameQuit?.Invoke();
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
            SIEventsHandler.OnObjectMovement?.Invoke();
        }

        public void OnGameStarted()
        {
            SIEventsHandler.OnGameStarted?.Invoke();
        }

        private void StartGame()
        {
            _isGameStarted = true;
        }
    }
}
