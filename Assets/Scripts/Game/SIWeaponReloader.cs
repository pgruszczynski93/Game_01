using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIWeaponReloader : MonoBehaviour
    {
        [SerializeField] SIWeaponHolderData _currentWeaponHolderData;
        [SerializeField] SIWeaponHolder[] _weaponHolders;

        int _projectileIndex;
        int _availableWeaponsCount;
        float _nextReloadTime;
        float _timeFromStart;
        
        SIWeaponHolder _currentWeaponHolder;
        Dictionary<WeaponTier, SIWeaponHolder> _allWeapons;
        
        void Start()
        {
            Initialise();
        }

        void Initialise()
        {
            if (_weaponHolders == null || _weaponHolders.Length == 0)
            {
                Debug.Log("No weapon holders attached.", this);
                return;
            }

            _allWeapons = new Dictionary<WeaponTier, SIWeaponHolder>
            {
                {WeaponTier.Tier_1, _weaponHolders[0]},
                {WeaponTier.Tier_2, _weaponHolders[1]},
            };

            UpdateCurrentWeaponTier(WeaponTier.Tier_1);
        }

        void UpdateCurrentWeaponTier(WeaponTier weaponTier)
        {
            _currentWeaponHolder = _allWeapons[weaponTier];
            _currentWeaponHolderData = _currentWeaponHolder.GetWeaponHolderData();
            _availableWeaponsCount = _currentWeaponHolderData.availableWeapons.Length;
        }

        public void Debug_UpdateWeapon(int weapon)
        {
            UpdateCurrentWeaponTier((WeaponTier)weapon);
        }

        void OnEnable()
        {
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        void AssignEvents()
        {
            SIEventsHandler.OnWeaponTierUpdate += UpdateCurrentWeaponTier;
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnWeaponTierUpdate -= UpdateCurrentWeaponTier;
        }

        public void TryToShootAndReload()
        {
            _timeFromStart = Time.time;
            if (_timeFromStart <= _nextReloadTime)
                return;

            _nextReloadTime = _timeFromStart + _currentWeaponHolderData.weaponHolderSettings.reloadTime;
            SelectNextProjectile();
        }

        void SelectNextProjectile()
        {
            _currentWeaponHolderData.availableWeapons[_projectileIndex].TryToLaunchWeaponEntities();
            ++_projectileIndex;
            if (_projectileIndex > _availableWeaponsCount - 1)
                _projectileIndex = 0;
        }
    }
}