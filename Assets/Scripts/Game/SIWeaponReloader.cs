using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIWeaponReloader : MonoBehaviour
    {
        [SerializeField] SIWeaponData _currentWeaponData;
        [SerializeField] SIWeaponHolder[] _weaponHolders;

        int _projectileIndex;
        int _availableWeaponsCount;
        float _nextReloadTime;
        
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
                {WeaponTier.Tier_1, _weaponHolders[0]}
            };

            UpdateCurrentWeaponTier(WeaponTier.Tier_1);
        }

        void UpdateCurrentWeaponTier(WeaponTier weaponTier)
        {
            _currentWeaponHolder = _allWeapons[weaponTier];
            _currentWeaponData = _currentWeaponHolder.GetWeaponData();
            _availableWeaponsCount = _currentWeaponData.availableWeapons.Length;
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
            float timeFromStart = Time.time;
            if (timeFromStart <= _nextReloadTime)
                return;

            _nextReloadTime = timeFromStart + _currentWeaponData.weaponSettings.reloadTime;
            SelectNextProjectile();
        }

        void SelectNextProjectile()
        {
            _currentWeaponData.availableWeapons[_projectileIndex].MoveProjectile();
            ++_projectileIndex;
            if (_projectileIndex > _availableWeaponsCount - 1)
                _projectileIndex = 0;
        }
    }
}