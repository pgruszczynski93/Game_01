using UnityEngine;

namespace SpaceInvaders
{
    public class SIShootBehaviour<T> : MonoBehaviour where T: MonoBehaviour
    {
        [SerializeField] protected T _projectileController;

        protected virtual void OnEnable() {}

        protected virtual void OnDisable() {}

        protected virtual void Shoot()
        {
            if (_projectileController == null)
            {
                SIHelpers.SISimpleLogger(this, "No projectile controller attached.", SimpleLoggerTypes.Error);
                return;
            }
        }
    }

}
