using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public static class WaitUtils
    {
        static readonly Dictionary<float, WaitForSeconds> WaitForSecondsCache = new Dictionary<float, WaitForSeconds>();

        static void TryToAddToCoroutineWaitCache(float waitTime)
        {
            WaitForSeconds wfs;
            if (WaitForSecondsCache.TryGetValue(waitTime, out wfs))
                return;

            wfs = new WaitForSeconds(waitTime);
            WaitForSecondsCache.Add(waitTime, wfs);
        }

        public static IEnumerator WaitForCachedSeconds(float waitTime)
        {
            TryToAddToCoroutineWaitCache(waitTime);

            yield return WaitForSecondsCache[waitTime];
        }

        public static IEnumerator WaitAndInvoke(float waitTime, Action onWaitFinished)
        {
            yield return WaitForCachedSeconds(waitTime);
            onWaitFinished?.Invoke();
        }

        public static IEnumerator SkipFrames(int totalFrames)
        {
            int framesToSkip = totalFrames;
            while (framesToSkip-- > 0) 
                yield return null;
        }

        public static IEnumerator SkipFramesAndInvoke(int frames, Action onWaitFinished)
        {
            yield return SkipFrames(frames);
            onWaitFinished?.Invoke();
        }

        public static IEnumerator SkipFramesAndRunSequence(Action onStart, Action onFinish, int frames) {
            onStart?.Invoke();
            yield return SkipFrames(frames);
            onFinish?.Invoke();
        }

        public static IEnumerator WaitSecondsAndRunSequence(Action onStart, Action onFinish, float seconds) {
            onStart?.Invoke();
            yield return WaitForCachedSeconds(seconds);
            onFinish?.Invoke();
        }
        
    }
}