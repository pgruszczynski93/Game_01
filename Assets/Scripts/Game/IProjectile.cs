using UnityEngine;

namespace SpaceInvaders
{
    public interface IProjectile
    {
        bool IsMoving { get; set; }

        WaitUntil WaitForProjectileReset { get; set; }
        MeshRenderer ProjectileMeshRenderer { get; set; }
    }
}