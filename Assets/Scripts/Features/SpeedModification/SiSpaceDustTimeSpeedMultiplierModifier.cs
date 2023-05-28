using PG.Game.EventSystem;
using UnityEngine;

namespace PG.Game.Features.SpeedModification {
    public class SiSpaceDustTimeSpeedMultiplierModifier : MonoBehaviour, IModifyTimeSpeedMultiplier {
        [SerializeField] ParticleSystem _particles;

        float _initialSimulationSpeed;
        ParticleSystem.MainModule _particlesMainModule;


        void Start() => Initialise();

        void Initialise() {
            _particlesMainModule = _particles.main;
            _initialSimulationSpeed = _particlesMainModule.simulationSpeed;
            RequestTimeSpeedModification();
        }

        public void RequestTimeSpeedModification() {
            GameplayEvents.BroadcastOnSpeedModificationRequested(this);
        }

        public void SetTimeSpeedModifier(float timeSpeedModifier, float progress) {
            _particlesMainModule.simulationSpeed = _initialSimulationSpeed * timeSpeedModifier;
        }
    }
}