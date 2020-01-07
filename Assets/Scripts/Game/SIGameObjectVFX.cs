using UnityEngine;

namespace SpaceInvaders
{
    public class SIGameObjectVFX : SIVFXBehaviour
    {
        [SerializeField] GameObject _effectVFX;

        protected Transform _effectTransform;
        protected override void Initialise()
        {
            _effectTransform = _effectVFX.transform;
        }

        protected override void ResetVFX()
        {
            _effectTransform.parent = _parentTransform;
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
                _effectTransform.parent = null;
        }

        protected override void ForceEnableVFX()
        {
            _effectVFX.SetActive(true);
        }

        protected override void ForceDisableVFX()
        {
            _effectVFX.SetActive(false);
        }
    }
}