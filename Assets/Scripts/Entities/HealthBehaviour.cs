using PG.Game.Configs;
using PG.Game.VFX;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PG.Game.Entities {
    public class HealthBehaviour : MonoBehaviour {
        [SerializeField] protected bool _isImmortal;
        [SerializeField] protected EntitySetup _entitySetup;
        [SerializeField] protected DamageVFX _damageVFX;

        protected float _currentHealth;
        protected float _healthPercent;
        protected float _healthLossPercent;

        protected void Start() {
            Initialise();
        }

        void Initialise() {
            _damageVFX.Initialise();
            SetMaxHealth();
        }

        [Button]
        public void SetMaxHealth() {
            _currentHealth = _entitySetup.entityMaxHealth;
            _damageVFX.ResetDamageVFX();
        }

        public void TryApplyDamage(float damage) {
            ModifyHealth(-damage);

            if (IsAlive())
                return;
            _damageVFX.ResetDamageVFX();
        }

        protected void TryRestoreHealth(float newHealth) {
            ModifyHealth(newHealth);

            if (HasFullHealth())
                _damageVFX.ResetDamageVFX();
        }

        void ModifyHealth(float newValue) {
            if (_isImmortal)
                return;

            _currentHealth += newValue;
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _entitySetup.entityMaxHealth);
            _healthPercent = _currentHealth / _entitySetup.entityMaxHealth;
            _healthLossPercent = 1 - _healthPercent;
            _damageVFX.SetDamageVFX(_healthLossPercent);
        }

        public bool IsAlive() {
            return _currentHealth > 0;
        }

        bool HasFullHealth() {
            return _currentHealth >= _entitySetup.entityMaxHealth;
        }
    }
}