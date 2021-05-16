using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "Weapon Config", menuName = "Mindwalker Studio/Projectile Config")]
    public class SIProjectileSetup : ScriptableObject
    {
        public SIProjectileSettings projectileSettings;
    }
}