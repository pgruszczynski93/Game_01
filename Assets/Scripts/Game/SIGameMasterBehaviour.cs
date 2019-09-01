using UnityEngine;

namespace SpaceInvaders
{
    public class SIGameMasterBehaviour : SIGenericSingleton<SIGameMasterBehaviour>
    {
        [SerializeField] private bool _isGameStarted;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private SIPlayerBehaviour _player;
        [SerializeField] private SIScreenAreaCalculator _screeenAreaCalculator;

        public Camera MainCamera
        {
            get
            {
                if (_mainCamera != null) 
                    return _mainCamera;
                
                SIHelpers.SISimpleLogger(this, "No camera assigned!", SimpleLoggerTypes.Error);
                return null;

            }
        }

        public SIPlayerBehaviour Player
        {
            get
            {
                if (_player != null)
                    return _player;

                SIHelpers.SISimpleLogger(this, "No player assigned to SIGameMasterBehaviour", SimpleLoggerTypes.Error);
                return null;
            }
        }

        public SIScreenAreaCalculator ScreenAreaCalculator => _screeenAreaCalculator;
        

        // Initialization in Start because, Awake is taken by signleton and some scripts may depend from it.
        protected void Start()
        {
            InitializeSpawners();
        }

        private void InitializeSpawners()
        {
            SIEventsHandler.BroadcastOnSpawnObject();
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
            HandleUpdate();
        }

        private void HandleUpdate()
        {
            OnUpdateIndependentMovements();
            
            if (_isGameStarted == false)
                return;
                
            OnUpdateMovements();
            OnShadersUpdateCallback();
            OnDebugInputHandling();
            OnGameQuitCallback();
        }

        private void OnUpdateIndependentMovements()
        {
            SIEventsHandler.BroadcastOnGameIndependentObjectsMovement();
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
            SIEventsHandler.BroadcastOnUpdate();
        }

        public void OnGameStarted()
        {
            SIEventsHandler.BroadcastOnGameStarted();
        }
    }
}