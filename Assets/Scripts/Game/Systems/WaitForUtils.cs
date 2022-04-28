using System;
using System.Collections;
using System.Threading;
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

        public static async UniTask WaitSecondsTask(float waitTime, CancellationToken cancellationToken, bool ignoreTimeScale = false) {
            await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: cancellationToken, ignoreTimeScale: ignoreTimeScale);
        }

        public static async UniTask WaitSecondsAndInvokeTask(float waitTime, Action onWaitFinished, CancellationToken cancellationToken, bool ignoreTimeScale = false) {
            await WaitSecondsTask(waitTime, cancellationToken, ignoreTimeScale);
            onWaitFinished?.Invoke();
        }

        public static async UniTask SkipFramesTask(int totalFrames, CancellationToken cancellationToken) {
            await UniTask.DelayFrame(totalFrames, cancellationToken: cancellationToken);
        }

        public static async UniTask SkipFramesAndInvokeTask(int frames, CancellationToken cancellationToken, Action onWaitFinished)
        {
            await SkipFramesTask(frames, cancellationToken);
            onWaitFinished?.Invoke();
        }

        public static async UniTask StartWaitSecFinishTask(Action onStart, Action onFinish, float seconds, CancellationToken cancellationToken) {
            onStart?.Invoke();
            await WaitSecondsTask(seconds, cancellationToken);
            onFinish?.Invoke();
        }
        
        public static async UniTask StartWaitFramesFinishTask(Action onStart, Action onFinish, int frames, CancellationToken cancellationToken) {
            onStart?.Invoke();
            await SkipFramesTask(frames, cancellationToken);
            onFinish?.Invoke();
        }
    }
}