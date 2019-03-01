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
            SetInitialReferences();
        }

        private void SetInitialReferences()
        {
            if (_availableProjectilesPrefabs == null)
            {
                Debug.LogError("Projectile list is not initialized.");
                return;
            }

            _currentProjectile = _availableProjectilesPrefabs[0];
        }

        public void SetCurrentProjectile(WeaponType weaponType)
        {
            int weapon = (int) weaponType;
            SIHelpers.SISimpleLogger(this, "Trying to change weapon from " + (int)_currentProjectile.projectileType + " to " + (int)weaponType, SimpleLoggerTypes.Log);
            if (weapon < (int)_currentProjectile.projectileType)
            {
                return;     // temporary remove it when alsers will be implemented 
            }
            SIHelpers.SISimpleLogger(this, "Weapon changed to " + weaponType, SimpleLoggerTypes.Log);
            int weaponIndex = weapon - 1;
            _currentProjectile = _availableProjectilesPrefabs[weaponIndex];
        }

        public void ResetCurrentProjectile()
        {
            _currentProjectile = _availableProjectilesPrefabs[0];
        }

        public void Shoot()
        {

            for (int i = 0; i < _projectileReloaders.Length; i++)
            {
                _projectileReloaders[i].ShootAndReloadProjectile();
            }
            //for (int i = 0; i < CurrentProjectile.projectilePrefabs.Length; i++)
            //{
            //    GameObject currentProjectile = CurrentProjectile.projectilePrefabs[i];
            //    currentProjectile.SetActive(true);
            //    currentProjectile.GetComponent<SIProjectileBehaviour>().MoveObj();
            //}
        }

        //DEBUG_
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                CurrentProjectile = _availableProjectilesPrefabs[0];
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                CurrentProjectile = _availableProjectilesPrefabs[1];
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                CurrentProjectile = _availableProjectilesPrefabs[2];
            }
        }
    }
}
