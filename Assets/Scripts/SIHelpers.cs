using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SpaceInvaders
{
    [System.Serializable]
    public class SimpleTween2DInfo
    {
        public float durationTime;
        public AnimationCurve animationCurve;
        [HideInInspector] public Vector3 startPos;
        [HideInInspector] public Vector3 endPos;
    }

    public static class SIHelpers
    {
        public static float CAMERA_MIN_VIEWPORT_X = 0.05f;
        public static float CAMERA_MAX_VIEWPORT_X = 0.95f;
        public static float CAMERA_MIN_VIEWPORT_Y = 0.0f;
        public static float CAMERA_MAX_VIEWPORT_Y = 1.0f;

        public static bool IsObjectInScreenHorizontalBounds2D(this Vector2 objectViewportPos)
        {
            if (objectViewportPos.x >= CAMERA_MAX_VIEWPORT_X || objectViewportPos.x <= CAMERA_MIN_VIEWPORT_X)
            {
                return true;
            }

            return false;
        }

        public static bool IsObjectInScreenVerticalBounds3D(this Vector3 objectViewportPos)
        {
            if(objectViewportPos.y >= CAMERA_MAX_VIEWPORT_Y || objectViewportPos.y <= CAMERA_MIN_VIEWPORT_Y)
            {
                return true;
            }

            return false;
        }

        public static Vector3 SnapToGrid(Vector3 pos, float offset)
        {
            float x = pos.x;
            float y = pos.y;
            float z = pos.z;
            x = SnapToGrid(x, offset);
            y = SnapToGrid(y, offset);
            z = SnapToGrid(z, offset);
            return new Vector3(x, y, z);
        }

        public static int SnapToGrid(int pos, int offset)
        {
            float x = pos;
            return Mathf.RoundToInt(x / offset) * offset;
        }

        public static float SnapToGrid(float pos, float offset)
        {
            float x = pos;
            return Mathf.Round(x / offset) * offset;
        }

        public static void UniqueList<T>(this List<T> list, T elementToInsert)
        {
            if (list.Contains(elementToInsert))
            {
                Debug.Log("List contains given element - update stopped.");
                return;
            }

            list.Add(elementToInsert);
        }

        public static void SISimpleLogger<T>(T sendingObject, string message, SimpleLoggerTypes logType) where T : MonoBehaviour
        {
#if UNITY_EDITOR
            string formattedMessage = string.Format("{0}(): {1}", typeof(T), message);
            switch (logType)
            {
                case SimpleLoggerTypes.Log:
                    Debug.Log(formattedMessage);
                    break;
                case SimpleLoggerTypes.Warning:
                    Debug.LogWarning(formattedMessage);
                    break;
                case SimpleLoggerTypes.Error:
                    Debug.LogError(formattedMessage);
                    break;
            }
#endif
        }

        public static IEnumerator SimpleTween3D(Action<Vector3> onTweenAction, SimpleTween2DInfo tweenInfo, Action onTweenEnd = null)
        {
            float currentTime = 0.0f;
            float animationProgress = 0.0f;
            float curveProgress = 0.0f;

            while (currentTime < tweenInfo.durationTime)
            {

                animationProgress = Mathf.Clamp01(currentTime / tweenInfo.durationTime);
                curveProgress = tweenInfo.animationCurve.Evaluate(animationProgress);

                onTweenAction?.Invoke(Vector3.Lerp(tweenInfo.startPos, tweenInfo.endPos,
                    currentTime / tweenInfo.durationTime));

                currentTime += Time.deltaTime;
                yield return null;
            }

            onTweenAction?.Invoke(tweenInfo.endPos);
            onTweenEnd.Invoke();
            yield return null;
        }
    }
}
