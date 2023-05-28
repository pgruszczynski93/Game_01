using UnityEngine;

namespace PG.Game.Configs {
    [CreateAssetMenu(fileName = "Laser Beam Damage Config", menuName = "Configs/Laser Beam Damage", order = 0)]
    public class LaserBeamDamageSettings : ScriptableObject {
        public int collisionCheckDistance;
        public float basicDamage;
        public float extraDamage;
    }
}