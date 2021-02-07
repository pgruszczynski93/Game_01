using SpaceInvaders;
using UnityEngine;

namespace Game.VFX {
    public class SISkyboxScroller : MonoBehaviour {
        
        static readonly int rotationId = Shader.PropertyToID("_Rotation");
        static readonly int rotationAxisId = Shader.PropertyToID("_RotationAxis");

        [SerializeField] float _scrollSpeed;

        float _currentAngle;
        
        void OnEnable() => SubscribeEvents();

        void OnDisable() => UnsubscribeEvents();

        void SubscribeEvents() {
            SIEventsHandler.OnIndependentUpdate += HandleOnIndependentUpdate;  
        }

        void UnsubscribeEvents() {
            SIEventsHandler.OnIndependentUpdate -= HandleOnIndependentUpdate;  
        } 

        void HandleOnIndependentUpdate() {
            RenderSettings.skybox.SetFloat(rotationId, GetClampedAngle());
        }

        float GetClampedAngle() {
            _currentAngle += Time.deltaTime * _scrollSpeed;
            return _currentAngle % SIMathUtils.FULL_EULER_ANGLE ;
        }
    }
}
