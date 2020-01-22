using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIWeaponHolder : MonoBehaviour
    {
        [SerializeField] WeaponHolderSetup weaponHolderSetup;
        [SerializeField] SIWeaponBehaviour[] _availableWeapons;

        public SIWeaponHolderData GetWeaponHolderData()
        {
            return new SIWeaponHolderData
            {
                weaponHolderSettings = weaponHolderSetup.weaponHolderSettings,
                availableWeapons = _availableWeapons
            };
        }
    }
}