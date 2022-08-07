using SpaceInvaders;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace Game.AsteroidsField {
    public class AsteroidsFieldPool : ObjectsPool<AsteroidsFieldVFX> {

        [SerializeField] Vector3 _vfxSpawnOffset;

        Vector3 _vfxSpawnPosition;

        protected override void Initialise() {
            base.Initialise();
            
            //VFX size is constant for each of pool elements.
            _vfxSpawnPosition = new Vector3(0,
                _currentlyPooledObject.VfxSize.y + _vfxSpawnOffset.y,
                _currentlyPooledObject.VfxSize.z + _vfxSpawnOffset.z);
        }

        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            SIGameplayEvents.OnWaveStart += HandleOnWaveStart;
        }

        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            SIGameplayEvents.OnWaveStart -= HandleOnWaveStart;
        }

        void HandleOnWaveStart(WaveType waveType) {
            if (waveType == WaveType.Asteroids) {
                ManagePoolableObject();
            }
        }

        protected override void ManagePoolableObject() {
            //It's used in 2 cases: Asteroid Wave starts & previous vfx is invisible.
            _currentlyPooledObject.gameObject.SetActive(true);
            _currentlyPooledObject.SetSpawnPosition(_vfxSpawnPosition);
            _currentlyPooledObject.PerformOnPoolActions();
        }
    }
}