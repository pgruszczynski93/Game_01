using UnityEngine;

namespace SpaceInvaders
{
    public class SISettings : MonoBehaviour
    {
        [SerializeField] bool _isEditorLoggingDisabled;
        [SerializeField] bool _isAndroidLoggingDisabled;

        private void OnEnable()
        {
            AssignEvents();
        }

        private void AssignEvents()
        {
            SIEventsHandler.OnGameStarted += SetTargetFrameRate;
            SIEventsHandler.OnGameStarted += TryToDisableLogging;
        }

        private void OnDisable()
        {
            RemoveEvents();
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnGameStarted -= SetTargetFrameRate;
            SIEventsHandler.OnGameStarted -= TryToDisableLogging;
        }


        private void SetTargetFrameRate()
        {
            Application.targetFrameRate = SISettingsConsts.APPLICATION_TARGET_FRAMERATE;
        }
        
        private void TryToDisableLogging()
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