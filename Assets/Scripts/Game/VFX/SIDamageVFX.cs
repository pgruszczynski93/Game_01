using UnityEngine;

namespace SpaceInvaders {
    public class SIDamageVFX : MonoBehaviour {

        const string FRESNEL_COLOR = "_FresnelColor";
        const string FRESNEL_POWER = "_FresnelPower";
        const string NOISE_SCALE = "_NoiseScale";
        const string NOISE_TRESHOLD = "_NoiseTreshold";
        const string EDGE_WIDTH = "_EdgeWidth";
        const string IS_COLOR_TINT_ACTIVE = "_IsColorTintActive";
        
        [SerializeField] Renderer _renderer;

        Material _material;
        static readonly int IsColorTintActive = Shader.PropertyToID(IS_COLOR_TINT_ACTIVE);

        void Start()
        {
            _material = _renderer.material;
        }

        public void SetDamageVFX()
        {
            
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                _material.SetInt(IsColorTintActive, 1);
            }
            
        }
    }
    
}
