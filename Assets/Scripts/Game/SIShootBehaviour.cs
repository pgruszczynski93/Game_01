using UnityEngine;

namespace SpaceInvaders
{
    public abstract class SIShootBehaviour : MonoBehaviour
    {
        [SerializeField] protected SIWeaponReloader weaponReloader;

        bool _initialised;

        protected virtual void Initialise()
        {
            if (_initialised)
                return;

            if (weaponReloader == null)
            {
                Debug.LogError("No projectile controller attached.", this);
                return;
            }
            
            _initialised = true;
        }

        void Start()
        {
            Initialise();
        }

        protected virtual void OnEnable()
        {
            AssignEvents();
        }

        protected virtual void OnDisable()
        {
            RemoveEvents();
        }

        protected abstract void TryToShootProjectile();
        
        protected virtual void AssignEvents() { }

        protected virtual void RemoveEvents() { }
    }
}