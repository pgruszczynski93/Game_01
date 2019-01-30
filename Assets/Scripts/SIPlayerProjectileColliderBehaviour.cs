namespace SpaceInvaders
{
    public class SIPlayerProjectileColliderBehaviour : SIMainColliderBehaviour<SIProjectileBehaviour>
    {
        protected override void OnEnable()
        {
            onCollisionCallback += _colliderParentBehaviour.ResetProjectile;
        }

        protected override void OnDisable()
        {
            onCollisionCallback -= _colliderParentBehaviour.ResetProjectile;
        }

    }
}
