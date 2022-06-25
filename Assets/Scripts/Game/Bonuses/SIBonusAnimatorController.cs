using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SpaceInvaders {
    public class SIBonusAnimatorController : MonoBehaviour, IModifyTimeSpeedMultiplier{
        enum BonusAnimationType {
            Show,
            Hide
        }
        
        // Note: Values aren't in <0,1> range to make sure that effect will be completed.
        const float NOT_DISSOLVED = 0f;    
        const float FULLY_DISSOLVED = 1f;
        
        static readonly int DissolveAmountID = Shader.PropertyToID("_DissolveAmount");
        static readonly int BonusCollectedID = Animator.StringToHash("BonusCollected");

        [SerializeField] float _showAnimationTime;
        [SerializeField] float _hideAnimationTime;
        [SerializeField] Animator _animator;

        bool _isVariantAnimationTriggered;
        bool _initialised;
        float _dissolveValue;

        Renderer _bonusVariantRenderer;
        MaterialPropertyBlock _propertyBlock;
        CancellationTokenSource _animationCancellation;
        
        public bool IsVariantAnimationTriggered => _isVariantAnimationTriggered;

        void TryInitialise() {
            if (_initialised)
                return;
            
            _isVariantAnimationTriggered = false;
            _initialised = true;
            _propertyBlock = new MaterialPropertyBlock();
            _dissolveValue = FULLY_DISSOLVED;
        }
        
        //Note: This code should runs once at Animation Event
        public void ShowBonusVariantAnimation() {
            RefreshCancellation();
            AnimationTask(BonusAnimationType.Show).Forget();
        }
        
        //Note: This code should runs once at Animation Event
        public void HideBonusVariantAnimation() {
            RefreshCancellation();
            AnimationTask(BonusAnimationType.Hide).Forget();
        }
        
        public void RequestTimeSpeedModification() {
            SIGameplayEvents.BroadcastOnSpeedModificationRequested(this);
        }

        public void SetTimeSpeedModifier(float timeSpeedModifier, float progress = 1) {
            SetSpeedModifier(timeSpeedModifier);
        }
        
        public void SetShowAnimation(Renderer variantRenderer) {
            TryInitialise();
            RefreshCancellation();
            _isVariantAnimationTriggered = true;
            _bonusVariantRenderer = variantRenderer;
            _animator.ResetTrigger(BonusCollectedID);
            ResetRendererPropertyBlock();
        }

        public void SetHideAnimation() {
            TryStopCurrentVariantAnimation();
            RefreshCancellation();
            _isVariantAnimationTriggered = true;
            _animator.SetTrigger(BonusCollectedID);
        }

        void ResetRendererPropertyBlock() {
            _bonusVariantRenderer.SetPropertyBlock(null);
        }

        void TryStopCurrentVariantAnimation() {
            _isVariantAnimationTriggered = false;
        }
        
        void UpdateSelectedFloatMaterialProperty(int propId, float newValue) {
            if (_bonusVariantRenderer == null)
                return;
            
            _bonusVariantRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(propId, newValue);
            _bonusVariantRenderer.SetPropertyBlock(_propertyBlock);
        }
        
        // Disclaimer: In this class, I wanted to use DoTween to make a smooth transition between dissolve states, but unfortunately it made a "disappearing mesh effect".
        // Tried to fix that bug, but any of the solutions didn't work.
        // ---====--- Instead I used coroutine.

        async UniTaskVoid AnimationTask(BonusAnimationType type) {
            try {
                if (_bonusVariantRenderer != null) {
                    int sign;
                    float currentTime = 0;
                    float progress;
                    float duration;
                    float dissolveStartValue;

                    if (type == BonusAnimationType.Show) {
                        duration = _showAnimationTime;
                        dissolveStartValue = FULLY_DISSOLVED;
                        sign = -1;
                    }
                    else {
                        duration = _hideAnimationTime;
                        dissolveStartValue = NOT_DISSOLVED;
                        sign = 1;
                    }

                    while (currentTime <= duration) {
                        progress = dissolveStartValue + sign * (currentTime / duration);

                        UpdateSelectedFloatMaterialProperty(DissolveAmountID, progress);
                        currentTime += Time.deltaTime;
                        await WaitUtils.SkipFramesTask(1, _animationCancellation.Token);
                    }

                    _isVariantAnimationTriggered = false;
                }
            }
            catch (OperationCanceledException) { }
        }

        public void SetSpeedModifier(float modifier) {
            //Note: I use as base animation speed value of 1.
            _animator.speed = modifier;
        }
        
        void RefreshCancellation() {
            _animationCancellation?.Cancel();
            _animationCancellation?.Dispose();
            _animationCancellation = new CancellationTokenSource();
        }
    }
}