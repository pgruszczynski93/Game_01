using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyWeaponReloader : SIWeaponReloader
    {
        protected override void Initialise()
        {
            if (_weaponHolders == null || _weaponHolders.Length == 0)
            {
                Debug.Log("No weapon holders attached.", this);
                return;
            }

//            var weaponTiers = Enum.GetValues(typeof(WeaponTier));
// todo: przeiterować po enumie

            _allWeapons = new Dictionary<WeaponTier, SIWeaponHolder>
            {
                {WeaponTier.Tier_1, _weaponHolders[0]},
            };
            
            ResetAllWeaponHolders();
            TryToUpdateCurrentWeaponTier(WeaponTier.Tier_1);
        }
        public override void TryToShootAndReload()
        {
            _currentWeaponHolderData
                .availableWeapons[_projectileIndex]
                .TryToLaunchWeaponEntities();
            
            SelectNextProjectile();
        }
    }
}
