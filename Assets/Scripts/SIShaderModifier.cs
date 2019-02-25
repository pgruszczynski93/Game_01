using UnityEngine;

namespace SpaceInvaders
{
    public class SIShaderModifier : MonoBehaviour
    {

        // to do: change on properties list
        
        [SerializeField] private string _shaderProperty;
        [SerializeField] private SIShaderModifierEffectInfo _effectInfo;
        [SerializeField] private Shader _shader;
        [SerializeField] private Renderer _renderer;

        private void Awake()
        {
            SetInitialReferences();
        }

        private void SetInitialReferences()
        {
            if (_shader == null || _renderer == null)
            {
                SIHelpers.SISimpleLogger(this, "Assign proper values first. ", SimpleLoggerTypes.Error);
                return;
            }

            _shaderProperty = "_EmissionPower";
        }

        private void OnEnable()
        {
            SIEventsHandler.OnShadersUpdate += ModifyShaderProperty;
        }

        private void OnDisable()
        {
            SIEventsHandler.OnShadersUpdate -= ModifyShaderProperty;
        }

        private void ModifyShaderProperty()
        {
            if (_shader == null || _renderer == null)
            {
                SIHelpers.SISimpleLogger(this, "Assign proper values first. ", SimpleLoggerTypes.Error);
                return;
            }

            _renderer.material.SetFloat(_shaderProperty, Mathf.PingPong(Time.time * _effectInfo.effectSpeed, 
                                                             _effectInfo.effectMaxValue) + _effectInfo.effectIntensity);
        }
    
    }
}
