using PG.Game.Collisions;
using PG.Game.EventSystem;
using PG.Game.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PG.Game.Entities.Player {
    public class PlayerBehaviour : GenericSingleton<PlayerBehaviour> {
        [SerializeField] GameObject[] _playerContent;
        [SerializeField] PlayerHealthBehaviour _playerHealthBehaviour;
        [SerializeField] PlayerMovementBehaviour _playerMovementBehaviour;

        void OnEnable() {
            SubscribeEvents();
        }

        void OnDisable() {
            UnsubscribeEvents();
        }

        void SubscribeEvents() {
            GameplayEvents.OnDamage += HandleOnDamage;
        }

        void UnsubscribeEvents() {
            GameplayEvents.OnDamage -= HandleOnDamage;
        }

        void HandleOnDamage(DamageInfo damageInfo) {
            if (this != damageInfo.ObjectToDamage)
                return;

            _playerHealthBehaviour.TryApplyDamage(damageInfo.Damage);
            CheckAliveStatus();
        }

        bool IsPlayerAlive() {
            return _playerHealthBehaviour.IsAlive();
        }

        void CheckAliveStatus() {
            if (IsPlayerAlive())
                return;

            // ManagePlayerContent(false);
            //todo: dodać event na restart i reset;
            // SIEventsHandler.BroadcastOnGameStateChanged(GameStates.GameFinished);
            GameplayEvents.BroadcastOnExplosiveObjectHit(_playerMovementBehaviour.MovementWorldPosition);
            _playerHealthBehaviour.SetMaxHealth();
        }

        void ManagePlayerContent(bool isEnabled) {
            for (int i = 0; i < _playerContent.Length; i++) _playerContent[i].SetActive(isEnabled);
        }

        [Button]
        void TestHealthEffect(int damage) {
            _playerHealthBehaviour.TryApplyDamage(damage);
            CheckAliveStatus();
        }
    }
}