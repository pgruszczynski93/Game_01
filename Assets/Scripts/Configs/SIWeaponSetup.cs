using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "Weapon Config", menuName = "Mindwalker Studio/Weapon Config")]
    public class SIWeaponSetup : ScriptableObject
    {
        public SIWeaponSettings weaponSettings;
    }
}