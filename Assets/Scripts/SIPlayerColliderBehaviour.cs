using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerColliderBehaviour : SIMainColliderBehaviour<SIPlayerBehaviour>
    {
        protected override void OnEnable()
        {
            onCollisionCallback += OnPlayerHitted;
        }

        protected override void OnDisable()
        {
            onCollisionCallback -= OnPlayerHitted;
        }


        private void OnPlayerHitted()
        {
            Debug.Log(" DOSTALEM!");
            SIEventsHandler.OnPlayerHit?.Invoke();
        }
    }
}
