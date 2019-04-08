using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    public class SIBonusColliderBehaviour : SIMainColliderBehaviour<SIBonusColliderBehaviour>
    {
        protected override void OnEnable()
        {
            _onCollisionActions["Player"] += OnPlayerInteraction;
        }

        protected override void OnDisable()
        {
            _onCollisionActions["Player"] -= OnPlayerInteraction;
        }

        private void OnPlayerInteraction(MonoBehaviour collisionBehaviour)
        {
            SIHelpers.SISimpleLogger(this, "Destroying bonus object.", SimpleLoggerTypes.Log);
            gameObject.SetActive(false);
        }
    }
}