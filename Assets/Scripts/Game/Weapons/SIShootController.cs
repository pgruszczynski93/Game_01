using UnityEngine;

namespace SpaceInvaders
{
    public abstract class SIShootController : MonoBehaviour
    {
        [SerializeField] protected SIProjectileSlotsParentController[] projectileSlotsParents;
        
        protected int _projectilesTier;

        protected void OnEnable() => SubscribeEvents();
        protected void OnDisable() => UnsubscribeEvents();
        
        protected virtual void SubscribeEvents() { }
        protected virtual void UnsubscribeEvents() { }

        public Transform[] GetProjectileSlotsParent() {
            return projectileSlotsParents[_projectilesTier].ProjectilesSlotsTransforms;
        }
    }
}