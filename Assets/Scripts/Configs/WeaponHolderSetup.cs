using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "Weapon Holder Config", menuName = "Mindwalker Studio/Weapon Holder Config")]

    public class WeaponHolderSetup : ScriptableObject
    {
        public WeaponHolderSettings weaponHolderSettings;
    }
}