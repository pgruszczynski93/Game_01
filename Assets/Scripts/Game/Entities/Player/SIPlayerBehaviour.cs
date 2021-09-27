using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerBehaviour : SIGenericSingleton<SIPlayerBehaviour>
    {
        [SerializeField] SIPlayerHealth _playerHealth;
        [SerializeField] SIPlayerMovement _playerMovement;
        [SerializeField] GameObject[] _playerContent;

        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents()
        {
            SIGameplayEvents.OnDamage += HandleOnDamage;
        }

        void UnsubscribeEvents()
        {
            SIGameplayEvents.OnDamage -= HandleOnDamage;
        }
        
        void HandleOnDamage(DamageInfo damageInfo) {
            if (this != damageInfo.ObjectToDamage)
                return;

            _playerHealth.TryApplyDamage(damageInfo.Damage);
            CheckAliveStatus();
        }

        bool IsPlayerAlive() {
            return _playerHealth.IsAlive();
        }
        
        void CheckAliveStatus() {
            if (IsPlayerAlive())
                return;

            // ManagePlayerContent(false);
            //todo: dodać event na restart i reset;
            // SIEventsHandler.BroadcastOnGameStateChanged(GameStates.GameFinished);
            SIGameplayEvents.BroadcastOnExplosiveObjectHit(_playerMovement.MovementWorldPosition);
            _playerHealth.SetMaxHealth();
        }
        
        void ManagePlayerContent(bool isEnabled)
        {
            for (int i = 0; i < _playerContent.Length; i++) {
                _playerContent[i].SetActive(isEnabled);
            }
        }
    }
}
