using System;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIWeaponHolder : MonoBehaviour
    {
        [SerializeField] WeaponSetup _weaponSetup;
        [SerializeField] SIProjectileBehaviour[] _availableWeapons;

        public SIWeaponData GetWeaponData()
        {
            return new SIWeaponData
            {
                weaponSettings = _weaponSetup.weaponSettings,
                availableWeapons = _availableWeapons
            };
        }
    }
}