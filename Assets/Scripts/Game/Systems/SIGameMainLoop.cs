using UnityEngine;

namespace SpaceInvaders {
    public class SIGameMainLoop : MonoBehaviour {
        
        [SerializeField] GameStates currentGameState;
        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents()
        {
            SIEventsHandler.OnGameStateChanged += HandleOnGameStateChanged;
            SIEventsHandler.OnGameStarted += HandleOnGameStarted;
            SIEventsHandler.OnGameQuit += HandleOnQuitGame;
        }

        void UnsubscribeEvents()
        {
            SIEventsHandler.OnGameStateChanged -= HandleOnGameStateChanged;
            SIEventsHandler.OnGameStarted -= HandleOnGameStarted;
            SIEventsHandler.OnGameQuit -= HandleOnQuitGame;
        }

        void HandleOnGameStateChanged(GameStates gameState)
        {
            currentGameState = gameState;
        }

        void HandleOnGameStarted()
        {
            _isGameStarted = true;
        }

        void Update()
        {
            HandleOnUpdate();
        }

        void HandleOnUpdate()
        {
            OnUpdateNoPlayableObjects();
            
            if (_isGameStarted == false)
                return;
                
            OnUpdateMovements();
            OnShadersUpdateCallback();
            OnDebugInputHandling();
            OnGameQuitCallback();
        }

        void OnUpdateNoPlayableObjects()
        {
            SIEventsHandler.BroadcastOnNonPlayableUpdate();
        }

        void OnDebugInputHandling()
        {
            SIEventsHandler.BroadcastOnDebugInputHandling();
        }

        void OnGameQuitCallback()
        {
            SIEventsHandler.BroadcastOnGameQuit();
        }

        void OnShadersUpdateCallback()
        {
            SIEventsHandler.BroadcastOnShadersUpdate();
        }

        void HandleOnQuitGame()
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

        void OnUpdateMovements()
        {
            SIEventsHandler.BroadcastOnUpdate();
        }

        public void OnGameStarted()
        {
            SIEventsHandler.BroadcastOnGameStarted();
        }
    }
    }
}