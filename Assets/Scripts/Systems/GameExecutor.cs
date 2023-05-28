using PG.Game.EventSystem;
using PG.Game.Helpers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PG.Game.Systems {
    public class GameExecutor : MonoBehaviour {
        public float defFdt;
        public float scale;

        [SerializeField] GameStates currentGameState;
        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();

        void SubscribeEvents() {
            GeneralEvents.OnGameStateChanged += HandleOnGameStateChanged;
        }

        void UnsubscribeEvents() {
            GeneralEvents.OnGameStateChanged -= HandleOnGameStateChanged;
        }

        void HandleOnGameStateChanged(GameStates gameState) {
            currentGameState = gameState;
            ManageGameStates();
        }

        public bool IsGameRunning() {
            return currentGameState == GameStates.GameStarted;
        }

        [Button]
        void TimeScaleTest() {
            Time.timeScale = scale;
            Time.fixedDeltaTime = defFdt * Time.timeScale;
        }

        void ManageGameStates() {
            switch (currentGameState) {
                case GameStates.GameOpened:
                    break;
                case GameStates.GameStarted:
                    break;
                case GameStates.GamePaused:
                    break;
                case GameStates.GameFinished:
                    break;
                case GameStates.GameQuit:
                    QuitGame();
                    break;
            }
        }

        void Update() {
            RunGameLoop();
        }

        void RunGameLoop() {
            GeneralEvents.BroadcastOnIndependentUpdate();

            if (!IsGameRunning())
                return;

            GeneralEvents.BroadcastOnUpdate();
        }

        void QuitGame() {
#if UNITY_ANDROID && !UNITY_EDITOR
                Application.Quit();
#elif UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}