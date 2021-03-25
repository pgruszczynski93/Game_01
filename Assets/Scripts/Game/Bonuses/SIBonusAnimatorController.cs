using System;
using DG.Tweening;
using UnityEngine;

namespace SpaceInvaders {
    public class SIBonusAnimatorController : MonoBehaviour {

        const float MIN_DISSOLVE = 0f;
        const float MAX_DISSOLVE = 1f;
        
        static readonly int DissolveAmountID = Shader.PropertyToID("_DissolveAmount");
        static readonly int BonusSpawnedID = Animator.StringToHash("BonusSpawned");
        static readonly int BonusCollectedID = Animator.StringToHash("BonusCollected");

        [SerializeField] float _variantAnimationTime;
        [SerializeField] Animator _animator;

        bool _isVariantRendererVisible; 
        float _dissolveAmountValue;
        
        Ease _ease;
        MeshRenderer _bonusVariantRenderer;
        MaterialPropertyBlock _propertyBlock;
        Coroutine _animationRoutine;
        Tweener _animationTweener;
        
        Action onAnimationStarted;
        Action onAnimationFinished;

        void Start() => Initialise();

        void Initialise() {
            _isVariantRendererVisible = false;
            _propertyBlock = new MaterialPropertyBlock();
            _dissolveAmountValue = MAX_DISSOLVE;
            _animationTweener = DOTween.To(() => _dissolveAmountValue, newVal => _dissolveAmountValue = newVal,
                    MIN_DISSOLVE, _variantAnimationTime)
                .OnUpdate(UpdateMaterialPropertyBlock)
                .SetEase(Ease.OutCubic)
                .SetAutoKill(false)
                .Pause();
        }

        public void AnimateBonusVariantRenderer() {
            if (_isVariantRendererVisible)
                return;

            _isVariantRendererVisible = true;
            _animationTweener?.Restart();
        }

        public void SetRendererToAnimate(MeshRenderer mRenderer) {
            _bonusVariantRenderer = mRenderer;
            _isVariantRendererVisible = false;
        }

        void UpdateMaterialPropertyBlock() {
            if (_propertyBlock == null)
                return;
            
            _bonusVariantRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(DissolveAmountID, _dissolveAmountValue);
            _bonusVariantRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}