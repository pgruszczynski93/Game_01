using System;

namespace SpaceInvaders
{
    public interface ICanCollide
    {
        Action OnCollisionDetected { get; set; }
        CollisionTag GetCollisionTag();
    }
}