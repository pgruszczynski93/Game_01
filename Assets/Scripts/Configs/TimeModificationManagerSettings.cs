using System;
using UnityEngine;

namespace Configs {
    [CreateAssetMenu(fileName = "Time Modification Manager Config",
        menuName = "Project/Time Modification Manager Config")]
    public class TimeModificationManagerSettings : ScriptableObject {
        public bool useIncrementalSpeedModification;
        public float defaultTimeSpeedMultiplier;
        public TimeSpeedMultiplierParameter basicTimeMultiplierParam;
        public TimeSpeedMultiplierParameter energyBoostTimeMultiplierParam;
        public AnimationCurve speedUpCurve;
        public AnimationCurve slowDownCurve;
    }

    [Serializable]
    public class TimeSpeedMultiplierParameter {
        [Range(0, 3)] public float duration;
        public float slowDownMultiplier;
        public float speedUpMultiplier;
        public float minMultiplier;
        public float maxMultiplier;
    }
}