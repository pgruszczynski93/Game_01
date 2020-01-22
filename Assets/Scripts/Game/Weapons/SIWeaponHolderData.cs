using System;

namespace SpaceInvaders
{
    [Serializable]
    public class SIWeaponHolderData
    {
        public WeaponHolderSettings weaponHolderSettings;
        public SIWeaponBehaviour[] availableWeapons;
    }
}