using UnityEngine;

namespace SpaceInvaders
{
    public abstract class SIShootController : MonoBehaviour
    {
        [SerializeField] protected SIProjectileSlotsParentController[] projectileSlotsParents;

        protected bool _isShootingEnabled;
        protected int _projectilesTier;
        public bool IsShootingEnabled => _isShootingEnabled; 

        protected void OnEnable() => SubscribeEvents();
        protected void OnDisable() => UnsubscribeEvents();
        
        protected virtual void SubscribeEvents() { }
        protected virtual void UnsubscribeEvents() { }

        public Transform[] GetProjectileSlotsParent() {
            return projectileSlotsParents[_projectilesTier].ProjectilesSlotsTransforms;
        }
        
        protected void SetShootingStatus(bool isEnabled) {
            _isShootingEnabled = isEnabled;
        }
    }
}