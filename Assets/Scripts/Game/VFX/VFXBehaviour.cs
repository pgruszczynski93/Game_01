using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders
{
    public class VFXBehaviour : MonoBehaviour, IPoolable {
        
        [SerializeField] bool _hasParticles;
        [SerializeField] Transform _thisTransform;
        [SerializeField] Transform _parent;
        [ShowIf("_hasParticles"), SerializeField] ParticleSystem _particles;
        
        Vector3 _currentDropPos;
        CancellationTokenSource _vfxCancellation;

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

        public void SetSpawnPosition(Vector3 spawnPos) {
            _currentDropPos = spawnPos;
            SetVfx();
        }

        public void SetSpawnRotation(Vector3 spawnRot) {
            //Intentionally unimplemented.
        }

        public void ManageScreenVisibility() {
            //Intentionally unimplemented - is always on screen.
        }

        public void PerformOnPoolActions() {
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
            _thisTransform.position = _currentDropPos;
        }

        void ResetVfx() {
            _thisTransform.SetParent(_parent);
            _thisTransform.localPosition = SIScreenUtils.HiddenObjectPosition;
            TryStopParticles();
        }

        async UniTaskVoid TryResetParticlesTask() {
            while (_particles.isPlaying)
                await WaitUtils.SkipFramesTask(1, _vfxCancellation.Token);
            
            ResetVfx();
        }
    }
}