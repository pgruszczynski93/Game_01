#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace PG.Game.Editor.Tools {
    public class LoggingOptionEditor : EditorWindow {
        public bool isEditorLoggingDisabled;
        public bool isAndroidLoggingDisabled;

        static LoggingOptionEditor loggerExtension;

        [MenuItem("Tools/Logger Extension")]
        public static void OpenWindow() {
            loggerExtension = GetWindow<LoggingOptionEditor>();
            loggerExtension.Show();
        }

        void OnGUI() {
            EnableLogging();
        }

        void EnableLogging() {
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