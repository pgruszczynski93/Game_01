using UnityEngine;

namespace Configs {
    [CreateAssetMenu(fileName = "Speed Modification Manager Config",
        menuName = "Project/Speed Modification Manager Config")]
    public class SpeedModificationManagerSettings : ScriptableObject {
        public bool useIncrementalSpeedModification;
        [Range(0, 3)] public float speedModificationDuration;
        public float slowDownMultiplier;
        public float speedUpMultiplier;
        public float defaultSpeedMultiplier;
        public AnimationCurve speedUpCurve;
        public AnimationCurve slowDownCurve;
    }
}