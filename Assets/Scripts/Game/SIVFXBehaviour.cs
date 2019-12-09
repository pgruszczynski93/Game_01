using UnityEngine;

namespace SpaceInvaders
{
    public abstract class SIVFXBehaviour : MonoBehaviour
    {
        [SerializeField] protected float _timeToReset;
        [SerializeField] protected Transform _parentTransform;
        public abstract void TryToManageVFX(bool canBeEnabled, bool canBeDetachedFromParent, bool shouldBeResetAfterUsage);
        protected virtual void Initialise() { }
        protected virtual void Start()
        {
            Initialise();
        }
        protected virtual void OnEnable() { }
        protected virtual void OnDisable() { }
        protected virtual void ForceEnableVFX() { }
        protected virtual void ForceDisableVFX() { }
        protected virtual void ResetVFX() { }
    }
}