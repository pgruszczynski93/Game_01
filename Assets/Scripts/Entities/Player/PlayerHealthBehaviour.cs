using PG.Game.Configs;
using PG.Game.EventSystem;
using PG.Game.Helpers;
using UnityEngine;

namespace PG.Game.Entities.Player {
    public class PlayerHealthBehaviour : HealthBehaviour {
        [SerializeField] ParticleSystem _healParticles;

        void OnEnable() {
            SubscribeEvents();
        }

        void OnDisable() {
            UnsubscribeEvents();
        }

        void SubscribeEvents() {
            BonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            BonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
        }

        void UnsubscribeEvents() {
            BonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            BonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
        }

        void HandleOnBonusEnabled(BonusSettings bonusSettings) {
            if (bonusSettings.bonusType != BonusType.Health)
                return;

            TryPlayHealEffect();
            TryRestoreHealth(bonusSettings.gainedHealth);
        }

        void HandleOnBonusDisabled(BonusSettings bonusSettings) {
            //Intentionally unimplemented (in case I forgot about some bonus disable event)
        }

        void TryPlayHealEffect() {
            if (_healParticles.isPlaying)
                return;
            _healParticles.Play();
        }

        public void ToggleImmortality() {
            _isImmortal = !_isImmortal;
        }
    }
}