using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] MeshRenderer _meshRenderer;
        [SerializeField] GameObject _colliderParent;
        [SerializeField] SIEnemyShootController _shootController;
        [SerializeField] SIEnemyHealth _enemyHealth;

        CancellationTokenSource _waitTaskCancellation;
        
        public SIEnemyShootController EnemyShootController
        {
            get => _shootController;
            set => _shootController = value;
        }

        void OnEnable() => SubscribeEvents();
        void OnDisable() =>UnsubscribeEvents();

        public void UpdateShootBehaviourSetup(SIShootBehaviourSetup loadedSetup)
        {
            _shootController.ShootBehaviourSetup = loadedSetup;
        }

        public bool IsEnemyAlive()
        {
            return _enemyHealth.IsAlive();
        }

        void SubscribeEvents()
        {
            SIGameplayEvents.OnWaveEnd += HandleOnWaveEnd;
            SIGameplayEvents.OnDamage += HandleOnDamage;
        }

        void UnsubscribeEvents()
        {
            SIGameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
            SIGameplayEvents.OnDamage -= HandleOnDamage;
        }

        void HandleOnWaveEnd(WaveType waveType)
        {
            if(waveType != WaveType.Grid)
                return;
            
            SetEnemyVisibility(true);
            _enemyHealth.SetMaxHealth();
        }

        void HandleOnDamage(DamageInfo damageInfo)
        {
            if (this != damageInfo.ObjectToDamage)
                return;

            _enemyHealth.TryApplyDamage(damageInfo.Damage);
            TryToBroadcastEnemyDeathAndSelectNextShootingEnemy();
        }
        
        void TryToBroadcastEnemyDeathAndSelectNextShootingEnemy()
        {
            if (IsEnemyAlive())
                return;

            if (_shootController.CanShoot)
                _shootController.TryToSelectNextShootingBehaviour();
            
            SetEnemyVisibility(false);
            RefreshCancellation();
            WaitUtils.SkipFramesAndInvokeTask(1, _waitTaskCancellation.Token, BroadcastEnemyDeath).Forget();
        }

        void BroadcastEnemyDeath()
        {
            //Note: This lines ensures explosion when enemy is killed.
            SIGameplayEvents.BroadcastOnExplosiveObjectHit(transform.position);
            SIGameplayEvents.BroadcastOnEnemyDeath(this);
        }
        
        void SetEnemyVisibility(bool isEnabled)
        {
            _colliderParent.SetActive(isEnabled);
            _meshRenderer.enabled = isEnabled;
        }
        
        void RefreshCancellation() {
            _waitTaskCancellation?.Cancel();
            _waitTaskCancellation?.Dispose();
            _waitTaskCancellation = new CancellationTokenSource();
        }
    }
}