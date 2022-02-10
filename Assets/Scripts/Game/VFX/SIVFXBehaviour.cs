using System.Collections;
using Sirenix.OdinInspector;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIVFXBehaviour : MonoBehaviour, IPoolable {
        
        [SerializeField] bool _hasParticles;
        [SerializeField] Transform _thisTransform;
        [SerializeField] Transform _parent;
        [ShowIf("_hasParticles"), SerializeField] ParticleSystem _particles;
        
        Vector3 _currentDropPos;
        Coroutine _particlesRoutine;

        void Initialise() {
            if (_parent == null) {
                _parent = transform.parent;
            }
        }
        void Start() => Initialise();

        void OnDestroy() {
            if(_particlesRoutine != null)
                StopCoroutine(_particlesRoutine);
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

        public void UseObjectFromPool() {
            TryPlayParticles();
        }
        
        void TryPlayParticles() {
            if (!CanPlayParticles())
                return;
            _particles.Play();
            _particlesRoutine = StartCoroutine(TryResetParticlesRoutine());
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

        IEnumerator TryResetParticlesRoutine() {
            while (_particles.isPlaying)
                yield return WaitUtils.SkipFrames(1);
            
            ResetVfx();
        }
    }
}