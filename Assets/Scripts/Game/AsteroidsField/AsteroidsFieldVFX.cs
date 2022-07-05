using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace Game.AsteroidsField {
    public class AsteroidsFieldVFX : MonoBehaviour, IPoolable {
        [SerializeField] GameObject _vfxParent;
        [SerializeField] ParticleSystem _particleSystem;

        void Initialise() {
            //todo: Poolaczyc te klase z SIVR
        }

        void Start() => Initialise();

        public void PerformOnPoolActions() {
        }

        public void SetSpawnPosition(Vector3 spawnPos) {
        }

        public void SetSpawnRotation(Vector3 spawnRot) {
        }

        public void ManageScreenVisibility() {
            
        }
        
        // void EnableVFXBehavior

        void StartPlayEffect() {
            _particleSystem.Play();
        }

        void StopPlayEffect() {
            _particleSystem.Stop();
        }
    }
}