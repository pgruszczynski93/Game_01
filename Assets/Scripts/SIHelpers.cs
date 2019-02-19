using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SpaceInvaders
{
    [Serializable]
    public class SimpleTweenInfo<T>
    {
        public float durationTime;
        public AnimationCurve animationCurve;
        [HideInInspector] public T startValue;
        [HideInInspector] public T endValue;
    }

    [Serializable]
    public class VectorTweenInfo : SimpleTweenInfo<Vector3>{}
    [Serializable]
    public class QuaternionTweenInfo : SimpleTweenInfo<Quaternion> {}

    public static class SIHelpers
    {
        public static float CAMERA_MIN_VIEWPORT_X = 0.075f;
        public static float CAMERA_MAX_VIEWPORT_X = 0.925f;
        public static float CAMERA_MIN_VIEWPORT_Y = 0.0f;
        public static float CAMERA_MAX_VIEWPORT_Y = 1.0f;
        public static Vector3 VectorZero = new Vector3(0f, 0f, 0f);

        public static Dictionary<float, WaitForSeconds> CoroutineWaitCache = new Dictionary<float, WaitForSeconds>();

        public static IEnumerator CustomDelayRoutine(float waitTime, Action onWaitFinished = null)
        {
            TryToAddToCoroutineWaitCache(waitTime);

            yield return CoroutineWaitCache[waitTime];
            Debug.Log("CZEKAM  + " +waitTime);
            onWaitFinished?.Invoke();
        }

        private static void TryToAddToCoroutineWaitCache(float waitTime)
        {
            WaitForSeconds wfs;
            if (CoroutineWaitCache.TryGetValue(waitTime, out wfs) == false)
            {
                wfs = new WaitForSeconds(waitTime);
                CoroutineWaitCache.Add(waitTime, wfs);
            }
        }

        public static bool IsObjectInScreenHorizontalBounds3D(this Vector3 objectViewportPos)
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

        public static void AddUnique<T>(this List<T> list, T elementToInsert)
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

        public static IEnumerator SimpleTween3D(Action<Vector3> onTweenAction, 
                                                VectorTweenInfo tweenInfo,
                                                Action onTweenEnd = null)
        {
            float currentTime = 0.0f;
            float animationProgress = 0.0f;
            float curveProgress = 0.0f;

            while (currentTime < tweenInfo.durationTime)
            {

                animationProgress = Mathf.Clamp01(currentTime / tweenInfo.durationTime);
                curveProgress = tweenInfo.animationCurve.Evaluate(animationProgress);

                onTweenAction?.Invoke(Vector3.Lerp(tweenInfo.startValue, tweenInfo.endValue,
                    currentTime / tweenInfo.durationTime));

                currentTime += Time.deltaTime;
                yield return null;
            }

            onTweenAction?.Invoke(tweenInfo.endValue);
            onTweenEnd.Invoke();
            yield return null;
        }

        public static IEnumerator SimpleTween3D(Action<Quaternion> onTweenAction,
            QuaternionTweenInfo tweenInfo,
            Action onTweenEnd = null)
        {
            float currentTime = 0.0f;
            float animationProgress = 0.0f;
            float curveProgress = 0.0f;

            while (currentTime < tweenInfo.durationTime)
            {
                animationProgress = Mathf.Clamp01(currentTime / tweenInfo.durationTime);
                curveProgress = tweenInfo.animationCurve.Evaluate(animationProgress);

                onTweenAction?.Invoke(Quaternion.Slerp(tweenInfo.startValue, tweenInfo.endValue,
                    currentTime / tweenInfo.durationTime));

                currentTime += Time.deltaTime;
                yield return null;
            }

            onTweenAction?.Invoke(tweenInfo.endValue);
            onTweenEnd?.Invoke();
            yield return null;
        }
    }
}
