using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public static class SIWaitUtils
    {
        private static Dictionary<float, WaitForSeconds> WaitForSecondsCache = new Dictionary<float, WaitForSeconds>(); 
        
        private static void TryToAddToCoroutineWaitCache(float waitTime)
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
            {
                yield return null;
            }
        }
    }
}