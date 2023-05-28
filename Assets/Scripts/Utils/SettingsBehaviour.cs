using PG.Game.EventSystem;
using UnityEngine;

namespace PG.Game.Helpers {
    public class SettingsBehaviour : MonoBehaviour {
        [SerializeField] bool _isEditorLoggingDisabled;
        [SerializeField] bool _isAndroidLoggingDisabled;

        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();

        void SubscribeEvents() {
            GeneralEvents.OnGameStateChanged += HandleOnGameStateChanged;
        }

        void UnsubscribeEvents() {
            GeneralEvents.OnGameStateChanged -= HandleOnGameStateChanged;
        }

        void HandleOnGameStateChanged(GameStates gameState) {
            if (gameState != GameStates.GameStarted)
                return;

            SetTargetFrameRate();
            TryToDisableLogging();
        }


        void SetTargetFrameRate() {
            Application.targetFrameRate = SettingsConsts.APPLICATION_TARGET_FRAMERATE;
        }

        void TryToDisableLogging() {
            Debug.unityLogger.logEnabled =
#if UNITY_ANDROID && !UNITY_EDITOR
                !_isAndroidLoggingDisabled;
#else
                !_isEditorLoggingDisabled;
#endif
        }
    }
}