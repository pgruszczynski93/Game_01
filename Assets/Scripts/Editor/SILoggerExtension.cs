#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MindwalkerStudio.Tools
{
    public class SILoggerExtension : EditorWindow
    {
        public bool isEditorLoggingDisabled;
        public bool isAndroidLoggingDisabled;

        private static SILoggerExtension loggerExtension;

        [MenuItem("Mindwalker Studio Tools/Logger Extension")]
        public static void OpenWindow()
        {
            loggerExtension = GetWindow<SILoggerExtension>();
            loggerExtension.Show();
        }

        private void OnGUI()
        {
            EnableLogging();
        }

        private void EnableLogging()
        {
            isEditorLoggingDisabled = !EditorGUILayout.Toggle("Disable EDITOR logging", isEditorLoggingDisabled);
            isAndroidLoggingDisabled = !EditorGUILayout.Toggle("Disable ANDROID logging", isAndroidLoggingDisabled);
            Debug.unityLogger.logEnabled =
#if UNITY_ANDROID && !UNITY_EDITOR
                isAndroidLoggingDisabled;
#else
                isEditorLoggingDisabled;
#endif
        }
    }
}
#endif