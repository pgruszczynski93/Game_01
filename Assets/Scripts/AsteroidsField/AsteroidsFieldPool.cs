using PG.Game.Systems.WaveSystem;
using PG.Game.EventSystem;
using PG.Game.Features.ObjectsPool;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PG.Game.AsteroidsField {
    public class AsteroidsFieldPool : ObjectsPool<AsteroidsFieldVFX> {
        [SerializeField] Vector3 _vfxSpawnOffset;

        Vector3 _vfxSpawnPosition;

        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            GameplayEvents.OnWaveStart += HandleOnWaveStart;
        }

        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            GameplayEvents.OnWaveStart -= HandleOnWaveStart;
        }

        void HandleOnWaveStart(WaveType waveType) {
            if (waveType == WaveType.Asteroids) {
                ManagePoolableObject();
            }
        }

        void UpdateVFXSpawnPosition() {
            //VFX size is constant for each of pool elements.
            //niech asteroidy spadaja w dol i sie pooluja
            _vfxSpawnPosition = new Vector3(0,
                _currentlyPooledObject.VfxSize.y + _vfxSpawnOffset.y,
                _currentlyPooledObject.VfxSize.z + _vfxSpawnOffset.z);
        }

        protected override void ManagePoolableObject() {
            //It's used in 2 cases: Asteroid Wave starts & previous vfx is invisible.
            _currentlyPooledObject.EnableParticlesGameObject(true);
            UpdateVFXSpawnPosition();
            _currentlyPooledObject.SetSpawnPosition(_vfxSpawnPosition);
            _currentlyPooledObject.PerformOnPoolActions();
        }

        [Button]
        void SetNext() {
            SetNextObjectFromPool();
            ManagePoolableObject();
        }
    }
}