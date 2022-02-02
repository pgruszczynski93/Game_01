using System;
using System.Collections;
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
        Coroutine _animationRoutine;
        
        Action onAnimationStarted;
        Action onAnimationFinished;

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
            _animationRoutine = StartCoroutine(AnimationRoutine(BonusAnimationType.Show));
        }
        
        //Note: This code should runs once at Animation Event
        public void HideBonusVariantAnimation() {
            _animationRoutine = StartCoroutine(AnimationRoutine(BonusAnimationType.Hide));
        }
        
        public void RequestTimeSpeedModification() {
            SIGameplayEvents.BroadcastOnSpeedModificationRequested(this);
        }

        public void SetTimeSpeedModifier(float timeSpeedModifier, float progress = 1) {
            SetSpeedModifier(timeSpeedModifier);
        }
        
        public void SetShowAnimation(Renderer variantRenderer) {
            TryInitialise();
            TryStopCurrentVariantAnimation();
            
            _isVariantAnimationTriggered = true;
            _bonusVariantRenderer = variantRenderer;
            _animator.ResetTrigger(BonusCollectedID);
            ResetRendererPropertyBlock();
        }

        public void SetHideAnimation() {
            TryStopCurrentVariantAnimation();
            
            _isVariantAnimationTriggered = true;
            _animator.SetTrigger(BonusCollectedID);
        }

        void ResetRendererPropertyBlock() {
            _bonusVariantRenderer.SetPropertyBlock(null);
        }

        void TryStopCurrentVariantAnimation() {
            if (_animationRoutine == null)
                return;

            _isVariantAnimationTriggered = false;
            StopCoroutine(_animationRoutine);
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

        IEnumerator AnimationRoutine(BonusAnimationType type) {
            if (_bonusVariantRenderer == null) {
                _isVariantAnimationTriggered = false;
                yield break;
            }

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
                progress = dissolveStartValue + sign * (currentTime/duration);
                
                UpdateSelectedFloatMaterialProperty(DissolveAmountID, progress);
                currentTime += Time.deltaTime;
                yield return WaitUtils.SkipFrames(1);
            }

            _isVariantAnimationTriggered = false;
        }

        public void SetSpeedModifier(float modifier) {
            //Note: I use as base animation speed value of 1.
            _animator.speed = modifier;
        }
    }
}