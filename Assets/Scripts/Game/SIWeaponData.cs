using System;

namespace SpaceInvaders
{
    [Serializable]
    public class SIWeaponData
    {
        public WeaponSettings weaponSettings;
        public SIProjectileBehaviour[] availableWeapons;
    }
}