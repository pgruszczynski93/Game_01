using SpaceInvaders;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Project.Systems {
    public class SIPostProcessController : MonoBehaviour, IModifyTimeSpeedMultiplier {
        [SerializeField] Volume _postProcessVolume;
         
        float _effectChangeDuration;
        Bloom _bloom;
        Vignette _vignette;
        
        //to do: dodać scriptable objecty zawierajace setup postprocesów

        void Start() => Initialise();

        void Initialise() {
            _postProcessVolume.profile.TryGet(out _bloom);
            _postProcessVolume.profile.TryGet(out _vignette);
        }
        
        public void SetTimeSpeedModifier(float modifier) {
            _effectChangeDuration = modifier;
        }
    }
}
