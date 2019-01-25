using UnityEngine;

namespace SpaceInvaders
{
    public class SIShootBehaviour : MonoBehaviour
    {
        [SerializeField] protected SIProjectilesController _projectileController;

        protected virtual void OnEnable() {}

        protected virtual void OnDisable() {}

        protected virtual void Shoot()
        {
            if (_projectileController == null)
            {
                Debug.Log("No projectile controller attached.");
                return;
            }
        }
    }

}
