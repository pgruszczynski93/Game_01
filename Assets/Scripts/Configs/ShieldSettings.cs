using UnityEngine;

namespace Configs {
    [CreateAssetMenu(fileName = "Shield Config", menuName = "Mindwalker Studio/Shield Config", order = 0)]
    public class ShieldSettings : ScriptableObject {
        public float waitForDisableTime;
        public Vector3 basicScale;
        public Vector3 extraScale;
    }
}