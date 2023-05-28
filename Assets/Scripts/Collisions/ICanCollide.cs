using System;
using PG.Game.Helpers;
using UnityEngine;

namespace PG.Game.Collisions {
    [Serializable]
    public class CollisionInfo {
        public CollisionTag collisionTag;
        public MonoBehaviour collisionSource;
    }

    public interface ICanCollide {
        bool IsCollisionTriggered { get; set; }
        Action<CollisionInfo> OnCollisionDetected { get; }
        CollisionInfo GetCollisionInfo();
    }
}