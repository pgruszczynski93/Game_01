using UnityEngine;

namespace SpaceInvaders {
    public class SIGameExecutor : MonoBehaviour {
        
        [SerializeField] GameStates currentGameState;
        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents()
        {
            SIEventsHandler.OnGameStateChanged += HandleOnGameStateChanged;
        }

        void UnsubscribeEvents()
        {
            SIEventsHandler.OnGameStateChanged -= HandleOnGameStateChanged;
        }

        void HandleOnGameStateChanged(GameStates gameState)
        {
            currentGameState = gameState;
            ManageGameStates();
        }

        public bool IsGameRunning()
        {
            return currentGameState == GameStates.GameStarted;
        }

        void ManageGameStates()
        {
            switch(currentGameState)
            {
                case GameStates.GameOpened:
                    break;
                case GameStates.GameInMenu:
                    break;
                case GameStates.GameWaitsForStart:
                    break;
                case GameStates.GameStarted:
                    break;
                case GameStates.GamePaused:
                    break;
                case GameStates.GameQuit:
                    QuitGame();
                    break;
            }
        }

        void Update()
        {
            RunGameLoop();
        }

        void RunGameLoop()
        {
            SIEventsHandler.BroadcastOnNonPlayableUpdate();
            
            if (!IsGameRunning())
                return;
                
            SIEventsHandler.BroadcastOnUpdate();
        }

        void QuitGame()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
                Application.Quit();
#elif UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}