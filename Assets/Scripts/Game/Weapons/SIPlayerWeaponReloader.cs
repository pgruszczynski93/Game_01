using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(SITargetFollower))]
    public class SIPlayerWeaponReloader : SIWeaponReloader
    {
        float _nextReloadTime;
        float _timeFromStart;

        protected override void Initialise()
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
                {WeaponTier.Tier_3, _weaponHolders[2]},
                {WeaponTier.Tier_4, _weaponHolders[3]}
            };
            ResetAllWeaponHolders();
            TryToUpdateCurrentWeaponTier(WeaponTier.Tier_1);
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
            SIEventsHandler.OnWeaponTierUpdate += HandleOnWeaponTierUpdate;
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
        }
        void RemoveEvents()
        {
            SIEventsHandler.OnWeaponTierUpdate -= HandleOnWeaponTierUpdate;
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
            if (bonusSettings.bonusDropInfo.bonusType != BonusType.Weapon)
                return;
            
            TryToUpdateCurrentWeaponTier((WeaponTier) bonusSettings.bonusProperties.bonusLevel);
        }
        
        void HandleOnBonusDisabled(BonusSettings bonusSettings)
        {
            if (bonusSettings.bonusDropInfo.bonusType != BonusType.Weapon)
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
