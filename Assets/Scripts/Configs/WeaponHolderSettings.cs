using UnityEngine;

namespace SpaceInvaders
{
    [System.Serializable]
    public class WeaponHolderSettings
    {
        public bool canBeUsedManyTimes;
        public float reloadTime;
        public WeaponTier weaponTier;
    }
}