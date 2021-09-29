using System;

namespace SpaceInvaders {
    public class SIPlayerHealth : SIHealth {

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
            TryRestoreHealth(bonusSettings.gainedHealth);
        }

        void HandleOnBonusDisabled(BonusSettings bonusSettings) {
            //Intentionally unimplemented (in case I forgot about some bonus disable event)
        }

        public void ToggleImmortality() {
            _isImmortal = !_isImmortal;
        }
    }
}