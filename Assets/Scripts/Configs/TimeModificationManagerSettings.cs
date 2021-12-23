using System;
using UnityEngine;

namespace Configs {
    [CreateAssetMenu(fileName = "Speed Modification Manager Config",
        menuName = "Project/Speed Modification Manager Config")]
    public class SpeedModificationManagerSettings : ScriptableObject {
        public bool useIncrementalSpeedModification;
        [Range(0, 3)] public float speedModificationDuration;
        [Range(0, 3)] public float energyBoostSpeedModificationDuration;
        public float slowDownMultiplier;
        public float speedUpMultiplier;
        public float defaultSpeedMultiplier;
        public float defaultTimeModificationValue;
        public float energyBoostTimeModificationValue;
        public AnimationCurve speedUpCurve;
        public AnimationCurve slowDownCurve;
    }

    [Serializable]
    public class SpeedModificationParameter {
        [Range(0, 3)] public float duration;
        public float slowDownMultiplier;
        public float speedUpMultiplier;
        
    }
}