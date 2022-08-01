using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders
{
    public class VFXBehaviour : MonoBehaviour, IPoolable {
        
        [SerializeField] protected bool _hasParticles;
        [SerializeField] protected Transform _thisTransform;
        [SerializeField] protected Transform _parent;
        [ShowIf("_hasParticles"), SerializeField] protected ParticleSystem _particles;
        
        protected Vector3 _currentSpawnPos;
        protected CancellationTokenSource _vfxCancellation;

        void Initialise() {
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

        public virtual void SetSpawnRotation(Vector3 spawnRot) {
            //Intentionally unimplemented.
        }

        public virtual void ManageScreenVisibility() {
            //Intentionally unimplemented.
        }

        public virtual void PerformOnPoolActions() {
            TryPlayParticles();
        }
        
        void TryPlayParticles() {
            if (!CanPlayParticles())
                return;
            _particles.Play();
            RefreshCancellation();
            TryResetParticlesTask().Forget();
        }

        void TryStopParticles() {
            if (!CanPlayParticles())
                return;
            _particles.Stop();
        }

        bool CanPlayParticles() {
            return _hasParticles && _particles != null && !_particles.isPlaying;
        }

        void SetVfx() {
            _thisTransform.position = _currentSpawnPos;
        }

        void ResetVfx() {
            _thisTransform.SetParent(_parent);
            _thisTransform.localPosition = SIScreenUtils.HiddenObjectPosition;
            TryStopParticles();
        }

        protected async UniTaskVoid TryResetParticlesTask() {
            while (_particles.isPlaying)
                await WaitUtils.SkipFramesTask(1, _vfxCancellation.Token);
            
            ResetVfx();
        }
    }
}