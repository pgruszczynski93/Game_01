using UnityEngine;

namespace Configs {
    [CreateAssetMenu(fileName = "Laser Beam Damage Config", menuName = "Mindwalker Studio/Laser Beam Damage Config", order = 0)]
    public class LaserBeamDamageSettings : ScriptableObject {
        public int collisionCheckDistance;
        public float basicDamage;
        public float extraDamage;
    }
}