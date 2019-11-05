using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "Weapon Config", menuName = "Mindwalker Studio/Weapon Config")]

    public class WeaponSetup : ScriptableObject
    {
        public WeaponSettings weaponSettings;
    }
}