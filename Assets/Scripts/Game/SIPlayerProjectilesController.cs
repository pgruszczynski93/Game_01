using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerProjectilesController : MonoBehaviour, IShootable
    {
        [SerializeField] private SIProjectileInfo _currentProjectile;
        [SerializeField] private List<SIProjectileInfo> _availableProjectilesPrefabs;
        [SerializeField] private SIProjectilesReloader[] _projectileReloaders;

        public List<SIProjectileInfo> AvailableProjectiles { get => _availableProjectilesPrefabs; set => _availableProjectilesPrefabs = value; }
        public SIProjectileInfo CurrentProjectile { get => _currentProjectile; set => _currentProjectile = value; }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_availableProjectilesPrefabs == null)
            {
                Debug.LogError("Projectile list is not initialized.");
                return;
            }

            _currentProjectile = _availableProjectilesPrefabs[0];
        }

        public void SetCurrentProjectile(CollectibleLevel collectibleLevel)
        {
            int weapon = (int) collectibleLevel;
            SIHelpers.SISimpleLogger(this, "Trying to change weapon from " + (int)_currentProjectile.projectileType + " to " + (int)collectibleLevel, SimpleLoggerTypes.Log);
            if (weapon < (int)_currentProjectile.projectileType)
            {
                return;     // temporary remove it when alsers will be implemented 
            }
            SIHelpers.SISimpleLogger(this, "Weapon changed to " + collectibleLevel, SimpleLoggerTypes.Log);
            int weaponIndex = weapon - 1;
            _currentProjectile = _availableProjectilesPrefabs[weaponIndex];
        }

        public void ResetCurrentProjectile()
        {
            _currentProjectile = _availableProjectilesPrefabs[0];
        }

        // poprawić tak by naboje wracaly do slotu dopiero po przekroczneiu ekranu a nie po smierci wroga
        public void Shoot()
        {

            for (int i = 0; i < _projectileReloaders.Length; i++)
            {
                _projectileReloaders[i].ShootAndReloadProjectile();
            }
        }
    }
}
