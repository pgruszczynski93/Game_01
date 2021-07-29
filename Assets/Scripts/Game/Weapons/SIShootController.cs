using UnityEngine;

namespace SpaceInvaders
{
    public abstract class SIShootController : MonoBehaviour
    {
        [SerializeField] protected SIWeaponReloader weaponReloader;
        // TO DO: przerobic to na tablice, tak zeby przy zmianie poziomu broni wroga, mozna było ustawić nowe sloty
        // niech shoot controller slucha na event ktory kaze zmienic aktualny tier pociskow
        [SerializeField] protected SIProjectileTierParentController projectileTierParentController;
        
        public SIProjectileTierParentController projectilesTierParentController => projectileTierParentController;

        protected abstract void TryToShootProjectile();

        protected void OnEnable() => SubscribeEvents();
        protected void OnDisable() => UnsubscribeEvents();
        
        protected virtual void SubscribeEvents() { }
        protected virtual void UnsubscribeEvents() { }
    }
}