using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIWeaponColliderBehaviour : SIColliderBehaviour, ICanCollide
    {
        [SerializeField] SIWeaponEntity _weaponEntity;
        [SerializeField] SIParticleSystemVFX _particlesVfx;
        public Action<CollisionInfo> OnCollisionDetected { get; set; }
        public CollisionInfo GetCollisionInfo()
        {
            return _thisCollisionInfo;
        }

        protected override void AssignEvents()
        {
            OnCollisionDetected += HandleOnCollisionDetected;
        }

        protected override void RemoveEvents()
        {
            OnCollisionDetected -= HandleOnCollisionDetected;
        }

        protected override void HandleOnCollisionDetected(CollisionInfo collisionInfo)
        {
            switch (collisionInfo.collisionTag)
            {
                case CollisionTag.Player:
//                    Debug.Log("Player hitted");
                    break;
                case CollisionTag.Enemy:
//                    Debug.Log("Enemy hitted");
                    SIGameplayEvents.BroadcastOnDamage(_weaponEntity.GetWeaponDamageInfo(collisionInfo.collisionSource));
                    break;
                case CollisionTag.PlayerWeapon:
//                    Debug.Log("PlayerWeapon hitted");
                    break;
                case CollisionTag.EnemyWeapon:
//                    Debug.Log("EnemyWeapon hitted");
                    break;
                case CollisionTag.Bonus:
//                    Debug.Log("Bonus hitted");
                    break;
                default:
                    break;
            }

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