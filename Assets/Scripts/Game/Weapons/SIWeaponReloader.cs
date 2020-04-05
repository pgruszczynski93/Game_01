using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public abstract class SIWeaponReloader : MonoBehaviour
    {
        [SerializeField] protected SIWeaponHolderData _currentWeaponHolderData;
        [SerializeField] protected SIWeaponHolder[] _weaponHolders;
        
        protected int _projectileIndex;
        protected int _availableWeaponsCount;
        protected WeaponTier _lastWeaponTier;
        protected SIWeaponHolder _currentWeaponHolder;
        protected Dictionary<WeaponTier, SIWeaponHolder> _allWeapons;

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
            
            _currentWeaponHolder = _allWeapons[weaponTier];
            _currentWeaponHolder.gameObject.SetActive(true);
            _projectileIndex = 0;
            _currentWeaponHolderData = _currentWeaponHolder.GetWeaponHolderData();
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