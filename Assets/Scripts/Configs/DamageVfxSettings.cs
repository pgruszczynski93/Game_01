using UnityEngine;

namespace PG.Game.Configs {
    [CreateAssetMenu(fileName = "Damage VFX config", menuName = "Configs/Damage VFX")]
    public class DamageVfxSettings : ScriptableObject {
        public DamageShaderSetup shaderSetup;
        public DamageVfxParticleSetup particleSetup;
    }

    [System.Serializable]
    public class DamageShaderSetup {
        public float minNoiseThreshold;
        public float maxNoiseThreshold;
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