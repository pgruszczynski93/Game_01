using UnityEngine;

namespace SpaceInvaders {
    public class SIPlayerHealth : SIHealth {

        [SerializeField] ParticleSystem _healParticles;
        
        void OnEnable() => SubscribeEvents();

        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents() {
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
        }
        
        void UnsubscribeEvents() {
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
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