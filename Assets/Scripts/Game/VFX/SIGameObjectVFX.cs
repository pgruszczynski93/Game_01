using UnityEngine;

namespace SpaceInvaders
{
    public class SIGameObjectVFX : SIVFXBehaviour
    {
        [SerializeField] protected Transform _effectTransform;
        
        [SerializeField] GameObject _effectVFX;

        protected override void Initialise()
        {
            _effectTransform = _effectVFX.transform;
        }

        protected override void ResetVFX()
        {
            _effectTransform.SetParent(_parentTransform);
            _effectTransform.localPosition = SIHelpers.VectorZero;
            ForceDisableVFX();
        }

        public override void TryToManageVFX(bool canBeEnabled, bool canBeDetachedFromParent, bool shouldBeResetAfterUsage)
        {
            if (_effectVFX == null)
                return;
            

            TryToDetachFromParent(canBeDetachedFromParent);
            TryToSetEffect(canBeEnabled);
            TryToResetEffectAfterUsage(shouldBeResetAfterUsage);
        }

        protected virtual void TryToSetEffect(bool canBeEnabled)
        {
            if (_effectVFX.activeSelf && canBeEnabled == false)
            {
                ForceDisableVFX();
                return;
            }

            _effectVFX.SetActive(canBeEnabled);
        }

        protected void TryToResetEffectAfterUsage(bool shouldBeResetAfterUsage)
        {
            if (shouldBeResetAfterUsage)
                StartCoroutine(SIWaitUtils.WaitAndInvoke(_timeToReset, ResetVFX));
        }

        protected void TryToDetachFromParent(bool canBeDetached)
        {
            if (canBeDetached)
                _effectTransform.SetParent(null);
        }

        protected virtual void ForceEnableVFX()
        {
            _effectVFX.SetActive(true);
        }

        protected virtual void ForceDisableVFX()
        {
            _effectVFX.SetActive(false);
        }
    }
}