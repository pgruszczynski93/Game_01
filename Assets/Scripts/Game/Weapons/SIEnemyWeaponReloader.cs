﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    [RequireComponent(typeof(SITargetFollower))]

    public class SIEnemyWeaponReloader : SIWeaponReloader
    {
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
            };
            
            ResetAllWeaponHolders();
            TryToUpdateCurrentWeaponTier(WeaponTier.Tier_1);
        }
        public override void TryToShootAndReload()
        {
            // for now index = 0 because enemy launches only one bullet each time.
            _currentWeaponHolderData
                .availableWeapons[_projectileIndex]
                .TryToLaunchWeaponEntities();
            
            SelectNextProjectile();
        }
    }
}
