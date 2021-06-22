using UnityEngine;

namespace SpaceInvaders
{
    public abstract class SIShootController : MonoBehaviour
    {
        [SerializeField] protected SIWeaponReloader weaponReloader;
        [SerializeField] protected SIProjectileRootController _projectileRootController;
        
        public SIProjectileRootController ProjectilesRootController => _projectileRootController;

        protected abstract void TryToShootProjectile();

        protected void OnEnable() => SubscribeEvents();
        protected void OnDisable() => UnsubscribeEvents();
        
        protected virtual void SubscribeEvents() { }
        protected virtual void UnsubscribeEvents() { }
    }
}