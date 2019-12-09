using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIWeaponColliderBehaviour : SIColliderBehaviour, ICanCollide
    {
        [SerializeField] SIWeaponEntity _weaponEntity;
        [SerializeField] SIParticleSystemVFX _particlesVfx;
        public Action OnCollisionDetected { get; set; }
        public CollisionTag GetCollisionTag()
        {
            return _collisionTag;
        }

        protected override void AssignEvents()
        {
            base.AssignEvents();
            OnCollisionDetected += HandleOnCollisionDetected;
        }

        protected override void RemoveEvents()
        {
            base.RemoveEvents();
            OnCollisionDetected -= HandleOnCollisionDetected;
        }

        protected override void HandleOnCollisionDetected()
        {
            DetectHit();
        }

        void DetectHit()
        {
            TryToDisplayVFX();
            _weaponEntity.HandleProjectileHit();
        }

        void TryToDisplayVFX()
        {
            if (_particlesVfx == null)
                return;

            _particlesVfx.TryToManageVFX(true, true, true);
        }
    }
}