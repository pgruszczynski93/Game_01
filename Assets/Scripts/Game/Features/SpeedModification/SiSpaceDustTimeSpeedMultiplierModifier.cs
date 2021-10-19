using UnityEngine;

namespace SpaceInvaders {
    public class SiSpaceDustTimeSpeedMultiplierModifier : MonoBehaviour, IModifyTimeSpeedMultiplier {

        [SerializeField] ParticleSystem _particles;

        float _initialSimulationSpeed;
        ParticleSystem.MainModule _particlesMainModule;
        
        
        void Start() => Initialise();

        void Initialise() {
            _particlesMainModule = _particles.main;
            _initialSimulationSpeed = _particlesMainModule.simulationSpeed;
            SIGameplayEvents.BroadcastOnSpeedModificationRequested(this);
        }

        public void SetTimeSpeedModifier(float modifier) {
            _particlesMainModule.simulationSpeed = _initialSimulationSpeed * modifier;
        }
    }
}