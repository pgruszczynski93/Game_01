using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIWeaponColliderBehaviour : SIColliderBehaviour, ICanCollide
    {
        [SerializeField] SIProjectileEntity projectileEntity;
        public bool IsCollisionTriggered { get; set; }
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
                    SIGameplayEvents.BroadcastOnDamage(projectileEntity.GetWeaponDamageInfo(collisionInfo.collisionSource));
                    break;
                case CollisionTag.PlayerWeapon:
//                    Debug.Log("PlayerWeapon hitted");
                    break;
                case CollisionTag.EnemyWeapon:
//                    Debug.Log("EnemyWeapon hitted");
                    SIGameplayEvents.BroadcastOnEnemyWeaponHit();
                    break;
                case CollisionTag.Bonus:
//                    Debug.Log("Bonus hitted");
                    break;
                default:
                    break;
            }

            ManageExplosionOnlyForEnemyWeapon(collisionInfo);

            projectileEntity.HandleProjectileHit();
        }

        void ManageExplosionOnlyForEnemyWeapon(CollisionInfo collisionInfo) {
            if (_thisCollisionInfo.collisionTag == CollisionTag.EnemyWeapon)
                TryDetectExplosiveHit(collisionInfo.collisionTag);
        }
    }
}