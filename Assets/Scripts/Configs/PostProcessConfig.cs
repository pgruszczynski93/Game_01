using UnityEngine;

namespace Configs {
    [CreateAssetMenu(fileName = "Post Process config", menuName = "Project/Post Process config")]
    public class PostProcessConfig : ScriptableObject {
        [Header("Bloom")] 
        public float bloomThreshold;
        public float bloomIntensity;
        public float bloomScatter;
        public Color bloomTintColor;

        [Header("Vignette"), Space] 
        public Color vignetteColor;
        public float vignetteIntensity;
        public float vignetteSmoothness;
    }
}