using System;
using DG.Tweening;
using UnityEngine;

namespace SpaceInvaders {
    public class SIBonusAnimatorController : MonoBehaviour {

        static readonly int DissolveAmountID = Shader.PropertyToID("DissolveAmount");
        static readonly int BonusSpawnedID = Animator.StringToHash("BonusSpawned");
        static readonly int BonusCollectedID = Animator.StringToHash("BonusCollected");

        [SerializeField] Animator _animator;

        Action onAnimationStarted;
        Action onAnimationFinished;

        MaterialPropertyBlock _propertyBlock;
        Coroutine _animationRoutine;

        bool _isMeshAnimationRunning = false;
        float _dissolveAmountValue;
        MeshRenderer _bonusVariantRenderer;

        public void SetRendererToAnimate(MeshRenderer mRenderer) {
            if(_propertyBlock == null)
                _propertyBlock = new MaterialPropertyBlock();

            // make it better!!!
            _dissolveAmountValue = 1;
            _bonusVariantRenderer = mRenderer;
        }
        
        public void AnimateBonusVariantRenderer() {
            if (_isMeshAnimationRunning)
                return;
            _isMeshAnimationRunning = true;
            DOTween.To(() => _dissolveAmountValue, newVal => _dissolveAmountValue = newVal, 0f, 1.5f) 
                .OnUpdate(UpdateMaterialPropertyBlock);
        }

        void UpdateMaterialPropertyBlock() {
            _bonusVariantRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(DissolveAmountID, _dissolveAmountValue);
            _bonusVariantRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}