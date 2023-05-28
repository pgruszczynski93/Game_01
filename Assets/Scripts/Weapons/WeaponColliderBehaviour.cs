using System;
using PG.Game.Collisions;
using PG.Game.EventSystem;
using PG.Game.Features;
using PG.Game.Helpers;
using PG.Game.Weapons.Projectile;
using UnityEngine;

namespace PG.Game.Weapons {
    public class WeaponColliderBehaviour : ColliderBehaviour, ICanCollide {
        [SerializeField] ProjectileBehaviour _projectileBehaviour;
        public bool IsCollisionTriggered { get; set; }
        public Action<CollisionInfo> OnCollisionDetected { get; set; }

        public CollisionInfo GetCollisionInfo() {
            return _thisCollisionInfo;
        }

        protected override void AssignEvents() {
            OnCollisionDetected += HandleOnCollisionDetected;
        }

        protected override void RemoveEvents() {
            OnCollisionDetected -= HandleOnCollisionDetected;
        }

        protected override void HandleOnCollisionDetected(CollisionInfo collisionInfo) {
            switch (collisionInfo.collisionTag) {
                case CollisionTag.Player:
//                    Debug.Log("Player hitted");
                    GameplayEvents.BroadcastOnDamage(_projectileBehaviour.GetWeaponDamageInfo(collisionInfo.collisionSource));
                    break;
                case CollisionTag.Enemy:
//                    Debug.Log("Enemy hitted");
                    GameplayEvents.BroadcastOnDamage(_projectileBehaviour.GetWeaponDamageInfo(collisionInfo.collisionSource));
                    break;
                case CollisionTag.PlayerWeapon:
//                    Debug.Log("PlayerWeapon hitted");
                    break;
                case CollisionTag.EnemyWeapon:
//                    Debug.Log("EnemyWeapon hitted");
                    GameplayEvents.BroadcastOnEnemyWeaponHit();
                    break;
                case CollisionTag.Bonus:
//                    Debug.Log("Bonus hitted");
                    break;
                default:
                    break;
            }

            ManageExplosionOnlyForEnemyWeapon(collisionInfo);

            _projectileBehaviour.HandleProjectileHit();
        }

        void ManageExplosionOnlyForEnemyWeapon(CollisionInfo collisionInfo) {
            if (_thisCollisionInfo.collisionTag == CollisionTag.EnemyWeapon)
                TryDetectExplosiveHit(collisionInfo.collisionTag);
        }
    }
}