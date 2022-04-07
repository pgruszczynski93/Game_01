using System;
using UnityEngine;

namespace Configs {
    [CreateAssetMenu(fileName = "Time Modification Manager Config",
        menuName = "Project/Time Modification Manager Config")]
    public class TimeModificationManagerSettings : ScriptableObject {
        public bool useIncrementalSpeedModification;
        public float defaultTimeSpeedMultiplier;
        public TimeModMultiplierSettings timeModSlowAllParam;
        public TimeModMultiplierSettings timeModFastAllParam;
        public TimeModMultiplierSettings timeWithEnergyBoostModParam;
        public AnimationCurve speedUpCurve;
        public AnimationCurve slowDownCurve;
    }

    [Serializable]
    public class TimeModMultiplierSettings {
        [Range(0, 3)] public float duration;
        public float targetTimeMultiplier;
        public float minTimeMultiplier;
        public float maxTimeMultiplier;
    }
}