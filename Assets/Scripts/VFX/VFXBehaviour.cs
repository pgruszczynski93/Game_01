using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PG.Game.Features.ObjectsPool;
using PG.Game.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PG.Game.VFX {
    public class VFXBehaviour : MonoBehaviour, IPoolable {
        [SerializeField] protected bool _hasParticles;
        [SerializeField] protected Transform _thisTransform;
        [SerializeField] protected Transform _parent;

        [ShowIf("_hasParticles"), SerializeField]
        protected ParticleSystem _particles;

        protected Vector3 _currentSpawnPos;
        protected CancellationTokenSource _vfxCancellation;

        protected virtual void Initialise() {
            if (_parent == null) {
                _parent = transform.parent;
            }
        }

        void Start() => Initialise();

        void RefreshCancellation() {
            _vfxCancellation?.Cancel();
            _vfxCancellation?.Dispose();
            _vfxCancellation = new CancellationTokenSource();
        }

        public virtual void SetSpawnPosition(Vector3 spawnPos) {
            _currentSpawnPos = spawnPos;
            SetVfx();
        }

        public virtual void SetSpawnRotation(Vector3 spawnRot) { }

        public virtual void ManageScreenVisibility() { }

        public virtual void PerformOnPoolActions() {
            TryPlayParticles();
        }

        public void EnableParticlesGameObject(bool enable) {
            _particles.gameObject.SetActive(enable);
        }

        void TryPlayParticles() {
            if (!PlayPossible())
                return;

            _particles.Play();
            RefreshCancellation();
            TryResetParticlesTask(() => !_particles.isPlaying).Forget();
        }

        void TryStopParticles() {
            if (!PlayPossible())
                return;

            _particles.Stop();
        }

        bool PlayPossible() {
            return _hasParticles && _particles != null && !_particles.isPlaying;
        }

        void SetVfx() {
            _thisTransform.position = _currentSpawnPos;
        }

        void ResetVfx() {
            _thisTransform.SetParent(_parent);
            _thisTransform.localPosition = ScreenUtils.HiddenObjectPosition;
            TryStopParticles();
        }

        protected async UniTaskVoid TryResetParticlesTask(Func<bool> onWait) {
            await UniTask.WaitUntil(onWait, cancellationToken: _vfxCancellation.Token);
            ResetVfx();
        }
    }
}