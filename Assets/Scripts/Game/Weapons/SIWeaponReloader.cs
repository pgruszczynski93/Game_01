using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public abstract class SIWeaponReloader : MonoBehaviour
    {
        [SerializeField] protected SIWeaponHolderData _currentWeaponHolderData;
        [SerializeField] protected SIProjectileTierParentController[] _weaponHolders;
        
        protected int _projectileIndex;
        protected int _availableWeaponsCount;
        protected WeaponTier _lastWeaponTier;
        protected SIProjectileTierParentController currentProjectileTierParentController;
        protected Dictionary<WeaponTier, SIProjectileTierParentController> _allWeapons;

        public abstract void TryToShootAndReload();
        
        void Start()
        {
            Initialise();
        }
        
        protected virtual void Initialise()
        {
            _lastWeaponTier = WeaponTier.Tier_0; // Tier_0 means no weapon: initialization.
        }

        protected void ResetAllWeaponHolders()
        {
            for (int i = 0; i < _weaponHolders.Length; i++)
            {
                _weaponHolders[i].gameObject.SetActive(false);
            }
        }

        protected void TryToUpdateCurrentWeaponTier(WeaponTier weaponTier)
        {
            if (_lastWeaponTier == weaponTier)
                return;
            
            currentProjectileTierParentController = _allWeapons[weaponTier];
            currentProjectileTierParentController.gameObject.SetActive(true);
            _projectileIndex = 0;
            // _currentWeaponHolderData = currentProjectileRootController.GetWeaponHolderData();
            _availableWeaponsCount = _currentWeaponHolderData.availableWeapons.Length;
            if (_lastWeaponTier != 0)
            {
                _allWeapons[_lastWeaponTier].gameObject.SetActive(false);
            }
            _lastWeaponTier = weaponTier;
        }
        
        protected void SelectNextProjectile()
        {
            _currentWeaponHolderData.availableWeapons[_projectileIndex].TryToLaunchWeaponEntities();
            ++_projectileIndex;
            if (_projectileIndex > _availableWeaponsCount - 1)
                _projectileIndex = 0;
        }
    }
}