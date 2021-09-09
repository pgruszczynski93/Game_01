using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerBehaviour : SIGenericSingleton<SIPlayerBehaviour>
    {
        [SerializeField] GameObject _playerContent;
        [SerializeField] SIPlayerHealth _playerHealth;

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

            ManagePlayerContent(false);
            //todo: dodać event na restart i reset;
            // SIEventsHandler.BroadcastOnGameStateChanged(GameStates.GameFinished);
        }
        
        void ManagePlayerContent(bool isEnabled)
        {
            _playerContent.SetActive(isEnabled);
        }
    }
}
