using UnityEngine;

namespace SpaceInvaders {
    public class SISettings : MonoBehaviour {
        [SerializeField] bool _isEditorLoggingDisabled;
        [SerializeField] bool _isAndroidLoggingDisabled;

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
            if (gameState != GameStates.GameStarted)
                return;
            
            SetTargetFrameRate();
            TryToDisableLogging();
        }


        void SetTargetFrameRate()
        {
            Application.targetFrameRate = SISettingsConsts.APPLICATION_TARGET_FRAMERATE;
        }

        void TryToDisableLogging()
        {
            Debug.unityLogger.logEnabled =
#if UNITY_ANDROID && !UNITY_EDITOR
                !_isAndroidLoggingDisabled;
#else
                !_isEditorLoggingDisabled;
#endif
        }
    }
}