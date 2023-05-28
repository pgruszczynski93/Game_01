using UnityEngine;

namespace PG.Game.Configs {
    [CreateAssetMenu(fileName = "Shield Config", menuName = "Configs/Shield", order = 0)]
    public class ShieldSettings : ScriptableObject {
        public float waitForDisableTime;
        public Vector3 basicScale;
        public Vector3 extraScale;
    }
}