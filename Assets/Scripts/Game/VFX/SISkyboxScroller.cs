using System;
using SpaceInvaders;
using UnityEngine;

namespace Game.VFX {
    public class SISkyboxScroller : MonoBehaviour, IModifyTimeSpeedMultiplier {
        
        static readonly int rotationId = Shader.PropertyToID("_Rotation");
        static readonly int rotationAxisId = Shader.PropertyToID("_RotationAxis");

        [SerializeField] float _scrollSpeed;

        float _currentScrollSpeedMultiplier;
        float _currentAngle;
        Material _skyboxBackup;

        void Start() => Initialise();
        void Initialise() {
            _skyboxBackup = new Material(RenderSettings.skybox);
            RequestTimeSpeedModification();
        }

        void OnDestroy() {
            RenderSettings.skybox = _skyboxBackup;
            DynamicGI.UpdateEnvironment();
            Destroy(_skyboxBackup);
        } 

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
            DynamicGI.UpdateEnvironment();
        }

        float GetClampedAngle() {
            _currentAngle += Time.deltaTime * _scrollSpeed * _currentScrollSpeedMultiplier;
            return _currentAngle % SIMathUtils.FULL_EULER_ANGLE ;
        }

        public void SetTimeSpeedModifier(float timeSpeedModifier, float progress) {
            _currentScrollSpeedMultiplier = timeSpeedModifier;
        }
        
        public void RequestTimeSpeedModification() {
            SIGameplayEvents.BroadcastOnSpeedModificationRequested(this);
        }
    }
}
