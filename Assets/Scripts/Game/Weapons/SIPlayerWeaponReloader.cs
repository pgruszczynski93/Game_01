using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerWeaponReloader : SIWeaponReloader
    {
        float _nextReloadTime;
        float _timeFromStart;

        protected override void Initialise()
        {
            LoadAttachedWeaponHolders();
            ResetAllWeaponHolders();
            TryToUpdateCurrentWeaponTier(WeaponTier.Tier_1);
        }

        void LoadAttachedWeaponHolders()
        {
            // todo: Don't remove - it skips first non weapon enum value
            WeaponTier[] weapons = (WeaponTier[]) Enum.GetValues(typeof(WeaponTier));
            _allWeapons = new Dictionary<WeaponTier, SIWeaponHolder>();
            for(int i = 1; i<weapons.Length; i++)
            {
                _allWeapons.Add(weapons[i], _weaponHolders[i-1]);
            }
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
            SIGameplayEvents.OnWeaponTierUpdate += HandleOnWeaponTierUpdate;
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
        }
        void RemoveEvents()
        {
            SIGameplayEvents.OnWeaponTierUpdate -= HandleOnWeaponTierUpdate;
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
        }

        void HandleOnWeaponTierUpdate(WeaponTier weaponTier)
        {
            TryToUpdateCurrentWeaponTier(weaponTier);
        }
        
        public void Debug_UpdateWeapon(int weapon)
        {
            TryToUpdateCurrentWeaponTier((WeaponTier) weapon);
        }

        void HandleOnBonusEnabled(BonusSettings bonusSettings)
        {
            if (bonusSettings.bonusType != BonusType.Weapon)
                return;
            
            TryToUpdateCurrentWeaponTier((WeaponTier) bonusSettings.bonusLevel);
        }
        
        void HandleOnBonusDisabled(BonusSettings bonusSettings)
        {
            if (bonusSettings.bonusType != BonusType.Weapon)
                return;
            
            TryToUpdateCurrentWeaponTier(WeaponTier.Tier_1);
        }
        
        public override void TryToShootAndReload()
        {
            _timeFromStart = Time.time;
            if (_timeFromStart <= _nextReloadTime)
                return;

            _nextReloadTime = _timeFromStart + _currentWeaponHolderData.weaponHolderSettings.reloadTime;
            SelectNextProjectile();
        }
    }
}
