using System;
using DG.Tweening;
using UnityEngine;

namespace SpaceInvaders {
    public class SIBonusAnimatorController : MonoBehaviour {

        static readonly int DissolveAmountID = Shader.PropertyToID("_DissolveAmount");
        static readonly int BonusSpawnedID = Animator.StringToHash("BonusSpawned");
        static readonly int BonusCollectedID = Animator.StringToHash("BonusCollected");

        [SerializeField] float _variantAnimationTime;
        [SerializeField] Animator _animator;

        Action onAnimationStarted;
        Action onAnimationFinished;

        float _dissolveAmountValue;
        MeshRenderer _bonusVariantRenderer;
        MaterialPropertyBlock _propertyBlock;
        Coroutine _animationRoutine;
        
        public void AnimateBonusVariantRenderer() {
            DOTween.To(() => _dissolveAmountValue, newVal => _dissolveAmountValue = newVal, 0f, _variantAnimationTime) 
                .OnUpdate(UpdateMaterialPropertyBlock);
        }

        public void SetRendererToAnimate(MeshRenderer mRenderer) {
            if(_propertyBlock == null)
                _propertyBlock = new MaterialPropertyBlock();

            // make it better!!!
            _dissolveAmountValue = 1;
            _bonusVariantRenderer = mRenderer;
        }

        void UpdateMaterialPropertyBlock() {
            _bonusVariantRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(DissolveAmountID, _dissolveAmountValue);
            _bonusVariantRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}