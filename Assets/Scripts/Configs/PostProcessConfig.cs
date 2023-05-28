using UnityEngine;

namespace PG.Game.Configs {
    [CreateAssetMenu(fileName = "Post Process config", menuName = "Configs/Postprocessing")]
    public class PostProcessConfig : ScriptableObject {
        public float effectApplyDuration;

        [Header("Bloom")] public float bloomThreshold;
        public float bloomIntensity;
        public float bloomScatter;
        public Color bloomTintColor;

        [Header("Vignette"), Space] public Color vignetteColor;
        public float vignetteIntensity;
        public float vignetteSmoothness;
    }
}