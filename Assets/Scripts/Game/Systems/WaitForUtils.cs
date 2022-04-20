using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SpaceInvaders
{
    public static class WaitForUtils
    {
        //Unitask - WaitForEndOfFrame/WaitForFixedUpdate/Coroutine is not supported.
        static readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
        
        public static IEnumerator SkipFixedFrames(int totalFrames) {
            int fixedFramesToSkip = totalFrames;
            while(fixedFramesToSkip-- > 0)
                yield return waitForFixedUpdate;
        }

        public static async UniTask WaitSecondsTask(float waitTime, bool ignoreTimeScale = false) {
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime), ignoreTimeScale);
        }

        public static async UniTask WaitSecondsAndInvokeTask(float waitTime, Action onWaitFinished, bool ignoreTimeScale = false) {
            await WaitSecondsTask(waitTime);
            onWaitFinished?.Invoke();
        }

        public static async UniTask SkipFramesTask(int totalFrames) {
            await UniTask.DelayFrame(totalFrames);
        }

        public static async UniTask SkipFramesAndInvokeTask(int frames, Action onWaitFinished)
        {
            await SkipFramesTask(frames);
            onWaitFinished?.Invoke();
        }

        public static async UniTask StartWaitSecFinishTask(Action onStart, Action onFinish, float seconds) {
            onStart?.Invoke();
            await WaitSecondsTask(seconds);
            onFinish?.Invoke();
        }
        
        public static async UniTask StartWaitFramesFinishTask(Action onStart, Action onFinish, int frames) {
            onStart?.Invoke();
            await SkipFramesTask(frames);
            onFinish?.Invoke();
        }
    }
}