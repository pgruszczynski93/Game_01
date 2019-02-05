using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerProjectilesController : MonoBehaviour, IShootable
    {
        [SerializeField] private SIProjectileInfo _currentProjectile;
        [SerializeField] private List<SIProjectileInfo> _availableProjectiles;

        public List<SIProjectileInfo> AvailableProjectiles { get => _availableProjectiles; set => _availableProjectiles = value; }
        public SIProjectileInfo CurrentProjectile { get => _currentProjectile; set => _currentProjectile = value; }

        private void Start()
        {
            SetInitialReferences();
        }

        private void SetInitialReferences()
        {
            if (_availableProjectiles == null)
            {
                Debug.LogError("Projectile list is not initialized.");
                return;
            }

            CurrentProjectile = _availableProjectiles[0];
        }

        public void SetCurrentProjectile(WeaponType weaponType)
        {
            int weapon = (int) weaponType;
            if (weapon > 3)
            {
                return;     // temporary remove it when alsers will be implemented 
            }
            _currentProjectile = _availableProjectiles[weapon];
        }

        public void Shoot()
        {
            for (int i = 0; i < CurrentProjectile.projectilePrefabs.Length; i++)
            {
                GameObject currentProjectile = CurrentProjectile.projectilePrefabs[i];
                currentProjectile.SetActive(true);
                currentProjectile.GetComponent<SIProjectileBehaviour>().MoveObj();
            }
        }

        //DEBUG_
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                CurrentProjectile = _availableProjectiles[0];
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                CurrentProjectile = _availableProjectiles[1];
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                CurrentProjectile = _availableProjectiles[2];
            }
        }
    }
}
