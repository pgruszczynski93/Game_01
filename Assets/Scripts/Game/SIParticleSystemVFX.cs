using UnityEngine;

namespace SpaceInvaders
{
    public class SIParticleSystemVFX : SIGameObjectVFX
    {
        [SerializeField] ParticleSystem _particles;
        
        protected override void Initialise()
        {
            base.Initialise();
            _particles.Stop();
        }

        public override void TryToManageVFX(bool canBeEnabled, bool canBeDetachedFromParent, bool shouldBeResetAfterUsage)
        {
            if (_particles == null)
                return;
            
            TryToDetachFromParent(canBeDetachedFromParent);
            TryToSetEffect(canBeEnabled);
            TryToResetEffectAfterUsage(shouldBeResetAfterUsage);
        }

        protected override void TryToSetEffect(bool canBeEnabled)
        {
            if (_particles.isPlaying && canBeEnabled == false)
            {
                ForceDisableVFX();
                return;
            }

            if (canBeEnabled)
            {
                _particles.Play();
            }
            else
            {
                _particles.Stop();
            }
        }

        protected override void ForceEnableVFX()
        {
            _particles.Play();
        }

        protected override void ForceDisableVFX()
        {
            _particles.Stop();
        }
    }
}