using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class SIBonusAnimatorController : MonoBehaviour {

        const float NOT_DISSOLVED = 0f;
        const float FULLY_DISSOLVED = 1f;
        
        static readonly int DissolveAmountID = Shader.PropertyToID("_DissolveAmount");
        static readonly int BonusSpawnedID = Animator.StringToHash("BonusSpawned");
        static readonly int BonusCollectedID = Animator.StringToHash("BonusCollected");

        [SerializeField] float _variantAnimationTime;
        [SerializeField] Animator _animator;

        bool _isVariantRendererVisible; 
        float _dissolveAmountValue;
        
        [SerializeField] Renderer _bonusVariantRenderer;
        MaterialPropertyBlock _propertyBlock;
        Coroutine _animationRoutine;
        Tweener _dissolveTweener;
        
        Action onAnimationStarted;
        Action onAnimationFinished;

        void Start() => Initialise();

        void Initialise() {
            _isVariantRendererVisible = false;
            _propertyBlock = new MaterialPropertyBlock();
            _dissolveTweener =
                DOTween.To(() => _dissolveAmountValue, newVal => _dissolveAmountValue = newVal,
                        NOT_DISSOLVED, _variantAnimationTime)
                    .OnUpdate(() => {
                        UpdateSelectedFloatMaterialProperty(DissolveAmountID, _dissolveAmountValue);
                    })
                    .SetAutoKill(false)
                    .Pause();
            
            _dissolveAmountValue = FULLY_DISSOLVED;
        }

        
        //Note: This code runs as Animation Event
        public void AnimateBonusVariantRenderer() {
            if (_isVariantRendererVisible )
                return;
            
            _isVariantRendererVisible = true;
            _dissolveAmountValue = FULLY_DISSOLVED;
            _dissolveTweener.Restart();
        }
        
        public void RunAnimation(Renderer variantRenderer) {;

            _dissolveAmountValue = FULLY_DISSOLVED;
            _isVariantRendererVisible = false;
            _bonusVariantRenderer = variantRenderer;
            ResetRendererPropertyBlock();
        }

        public void ResetAnimation() {
            _dissolveTweener.Pause();
            ResetRendererPropertyBlock();
        }

        void ResetRendererPropertyBlock() {
            //EDGE WIDTH + minmaxy do poprawki
            _bonusVariantRenderer.SetPropertyBlock(null);
        }
        
        void UpdateSelectedFloatMaterialProperty(int propId, float newValue) {
            if (_bonusVariantRenderer == null)
                return;
            
            _bonusVariantRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(propId, newValue);
            _bonusVariantRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}