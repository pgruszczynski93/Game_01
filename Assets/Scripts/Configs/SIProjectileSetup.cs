using UnityEngine;

namespace PG.Game.Configs {
    [CreateAssetMenu(fileName = "Weapon Config", menuName = "Configs/Projectile")]
    public class SIProjectileSetup : ScriptableObject {
        public SIProjectileSettings projectileSettings;
    }
}