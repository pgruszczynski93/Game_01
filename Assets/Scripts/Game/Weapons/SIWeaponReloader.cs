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
        WeaponTier _lastWeaponTier;

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

            _lastWeaponTier = WeaponTier.Tier_0; // Tier_0 means no weapon: initialization.
            _allWeapons = new Dictionary<WeaponTier, SIWeaponHolder>
            {
                {WeaponTier.Tier_1, _weaponHolders[0]},
                {WeaponTier.Tier_2, _weaponHolders[1]},
                {WeaponTier.Tier_3, _weaponHolders[2]},
                {WeaponTier.Tier_4, _weaponHolders[3]}
            };
            ResetAllWeaponHolders();
            TryToUpdateCurrentWeaponTier(WeaponTier.Tier_1);
        }

        void ResetAllWeaponHolders()
        {
            for (var i = 0; i < _weaponHolders.Length; i++)
            {
                _weaponHolders[i].gameObject.SetActive(false);
            }
        }
        void TryToUpdateCurrentWeaponTier(WeaponTier weaponTier)
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

        public void Debug_UpdateWeapon(int weapon)
        {
            TryToUpdateCurrentWeaponTier((WeaponTier) weapon);
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
            SIEventsHandler.OnWeaponTierUpdate += TryToUpdateCurrentWeaponTier;
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnWeaponTierUpdate -= TryToUpdateCurrentWeaponTier;
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
        }
        
        void HandleOnBonusEnabled(BonusSettings bonusSettings)
        {
            if (bonusSettings.bonusType != BonusType.Weapon)
                return;
            
            TryToUpdateCurrentWeaponTier((WeaponTier) bonusSettings.bonusProperties.bonusLevel);
        }
        
        void HandleOnBonusDisabled(BonusSettings bonusSettings)
        {
            if (bonusSettings.bonusType != BonusType.Weapon)
                return;
            
            TryToUpdateCurrentWeaponTier(WeaponTier.Tier_1);
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