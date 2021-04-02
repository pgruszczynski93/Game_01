using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class SIBonusAnimatorController : MonoBehaviour {
        enum AnimationState {
            Show,
            Hide
        }
        
        const float NOT_DISSOLVED = 0f;    
        const float FULLY_DISSOLVED = 1f;
        
        static readonly int DissolveAmountID = Shader.PropertyToID("_DissolveAmount");
        static readonly int BonusSpawnedID = Animator.StringToHash("BonusSpawned");
        static readonly int BonusCollectedID = Animator.StringToHash("BonusCollected");

        [SerializeField] float _showAnimationTime;
        [SerializeField] float _hideAnimationTime;
        [SerializeField] Animator _animator;

        bool _initialised;
        bool _isVariantRendererVisible; 
        float _dissolveValue;
        
        Renderer _bonusVariantRenderer;
        MaterialPropertyBlock _propertyBlock;
        Coroutine _animationRoutine;
        
        Action onAnimationStarted;
        Action onAnimationFinished;

        void TryInitialise() {
            if (_initialised)
                return;
            
            _initialised = true;
            _isVariantRendererVisible = false;
            _propertyBlock = new MaterialPropertyBlock();
            _dissolveValue = FULLY_DISSOLVED;
        }
        
        
        //Note: This code runs as Animation Event
        public void RunShowAnimation() {
            if (_isVariantRendererVisible )
                return;

            _isVariantRendererVisible = true;
            _animationRoutine = StartCoroutine(AnimationRoutine(AnimationState.Show));
        }
        
        
        public void ReloadAnimation(Renderer variantRenderer) {
            TryInitialise();
             if (_animationRoutine != null)
                StopCoroutine(_animationRoutine);
             
            _isVariantRendererVisible = false;
            _bonusVariantRenderer = variantRenderer;
            ResetRendererPropertyBlock();
        }

        void ResetRendererPropertyBlock() {
            _bonusVariantRenderer.SetPropertyBlock(null);
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

        IEnumerator AnimationRoutine(AnimationState state) {
            if (_bonusVariantRenderer == null)
                yield break;

            int sign;
            float currentTime = 0;
            float progress;
            float duration;
            float dissolveStartValue;
            
            if (state == AnimationState.Show) {
                duration = _showAnimationTime;
                dissolveStartValue = FULLY_DISSOLVED;
                sign = -1;
            }
            else {
                duration = _hideAnimationTime;
                dissolveStartValue = NOT_DISSOLVED;
                sign = 1;
            }
            
            while (currentTime < duration) {
                progress = dissolveStartValue + sign * (currentTime/duration);
                
                UpdateSelectedFloatMaterialProperty(DissolveAmountID, progress);
                currentTime += Time.deltaTime;
                yield return WaitUtils.SkipFrames(1);
            }
        }
    }
}