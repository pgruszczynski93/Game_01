using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIProjectilesController : MonoBehaviour, IShootable
    {
        [SerializeField] private SIProjectileInfo _currentProjectile;
        [SerializeField] private List<SIProjectileInfo> _availableProjectiles;

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

            _currentProjectile = _availableProjectiles[0];
        }

        public void Shoot()
        {
            for (int i = 0; i < _currentProjectile.projectilePrefabs.Length; i++)
            {
                GameObject currentProjectile = _currentProjectile.projectilePrefabs[i];
                currentProjectile.SetActive(true);
            }
        }
    }
}
