using UnityEngine;

namespace ScriptableSettings {
    [CreateAssetMenu(fileName = "Damage VFX config", menuName = "Mindwalker Studio/Damage VFX config")]
    public class DamageVfxSettings : ScriptableObject {
        public DamageShaderSetup shaderSetup;
        public DamageVfxParticleSetup particleSetup;
    }

    [System.Serializable]
    public class DamageShaderSetup {
        public float minNoiseTreshold;
        public float maxNoiseTreshold;
        public float minEdgeWidth;
        public float maxEdgeWidth;
        public float noiseChangeDuration;
        public float edgeChangeDuration;
    }

    [System.Serializable]
    public class DamageVfxParticleSetup {
        public int minParticlesCount;
        public int maxParticlesCount;
        public float scaleChangeDuration;
        public float minScaleAxisValue;
        public float maxScaleAxisValue;
    }
}