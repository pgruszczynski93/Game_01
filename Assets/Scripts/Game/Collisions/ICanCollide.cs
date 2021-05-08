using System;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public class CollisionInfo
    {
        public CollisionTag collisionTag;
        public MonoBehaviour collisionSource;
    }
    public interface ICanCollide
    {
        bool IsCollisionTriggered { get; set; }
        Action<CollisionInfo> OnCollisionDetected { get; }
        CollisionInfo GetCollisionInfo();
    }
}