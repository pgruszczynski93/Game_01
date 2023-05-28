using PG.Game.EventSystem;
using PG.Game.Features;
using PG.Game.Systems;
using UnityEngine;

namespace PG.Game.VFX {
    public class SkyboxScroller : MonoBehaviour, IModifyTimeSpeedMultiplier {
        static readonly int xRotationId = Shader.PropertyToID("_Rotation");
        static readonly int rotationAxisId = Shader.PropertyToID("_RotationAxis");

        [SerializeField] float _scrollSpeed;

        float _currentScrollSpeedMultiplier;
        float _currentAngle;
        float _startSkyboxAngleX;
        Material _skyboxBackup;

        void Start() => Initialise();

        void Initialise() {
            _skyboxBackup = new Material(RenderSettings.skybox);
            RequestTimeSpeedModification();
        }

        void OnDestroy() {
            Destroy(_skyboxBackup);
        }

        void OnEnable() {
            BackupMaterialProperties();
            SubscribeEvents();
        }

        void OnDisable() {
            RestoreOriginalMaterialProperties();
            UnsubscribeEvents();
        }

        void SubscribeEvents() {
            GeneralEvents.OnIndependentUpdate += HandleOnIndependentUpdate;
        }

        void UnsubscribeEvents() {
            GeneralEvents.OnIndependentUpdate -= HandleOnIndependentUpdate;
        }

        void BackupMaterialProperties() {
            _startSkyboxAngleX = RenderSettings.skybox.GetFloat(xRotationId);
        }

        void RestoreOriginalMaterialProperties() {
            RenderSettings.skybox = _skyboxBackup;
            RenderSettings.skybox.SetFloat(xRotationId, _startSkyboxAngleX);
        }

        void HandleOnIndependentUpdate() {
            RenderSettings.skybox.SetFloat(xRotationId, GetClampedAngle());
            DynamicGI.UpdateEnvironment();
        }

        float GetClampedAngle() {
            _currentAngle += Time.deltaTime * _scrollSpeed * _currentScrollSpeedMultiplier;
            return _currentAngle % MathUtils.FULL_EULER_ANGLE;
        }

        public void SetTimeSpeedModifier(float timeSpeedModifier, float progress) {
            _currentScrollSpeedMultiplier = timeSpeedModifier;
        }

        public void RequestTimeSpeedModification() {
            GameplayEvents.BroadcastOnSpeedModificationRequested(this);
        }
    }
}