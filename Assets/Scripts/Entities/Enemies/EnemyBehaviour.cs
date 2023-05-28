using System.Threading;
using Cysharp.Threading.Tasks;
using PG.Game.Collisions;
using PG.Game.Systems.WaveSystem;
using PG.Game.EventSystem;
using PG.Game.Systems;
using UnityEngine;

namespace PG.Game.Entities.Enemies {
    public class EnemyBehaviour : MonoBehaviour {
        [SerializeField] MeshRenderer _meshRenderer;
        [SerializeField] GameObject _colliderParent;
        [SerializeField] EnemyShootController _shootController;
        [SerializeField] EnemyHealthBehaviour _enemyHealthBehaviour;

        CancellationTokenSource _waitTaskCancellation;

        public EnemyShootController EnemyShootController {
            get => _shootController;
            set => _shootController = value;
        }

        void OnEnable() {
            SubscribeEvents();
        }

        void OnDisable() {
            UnsubscribeEvents();
        }

        public void UpdateShootBehaviourSetup(SIShootBehaviourSetup loadedSetup) {
            _shootController.ShootBehaviourSetup = loadedSetup;
        }

        public bool IsEnemyAlive() {
            return _enemyHealthBehaviour.IsAlive();
        }

        void SubscribeEvents() {
            GameplayEvents.OnWaveEnd += HandleOnWaveEnd;
            GameplayEvents.OnDamage += HandleOnDamage;
        }

        void UnsubscribeEvents() {
            GameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
            GameplayEvents.OnDamage -= HandleOnDamage;
        }

        void HandleOnWaveEnd(WaveType waveType) {
            SetEnemyVisibility(true);
            _enemyHealthBehaviour.SetMaxHealth();
        }

        void HandleOnDamage(DamageInfo damageInfo) {
            if (this != damageInfo.ObjectToDamage)
                return;

            _enemyHealthBehaviour.TryApplyDamage(damageInfo.Damage);
            TryToBroadcastEnemyDeathAndSelectNextShootingEnemy();
        }

        void TryToBroadcastEnemyDeathAndSelectNextShootingEnemy() {
            if (IsEnemyAlive())
                return;

            if (_shootController.CanShoot)
                _shootController.TryToSelectNextShootingBehaviour();

            SetEnemyVisibility(false);
            RefreshCancellation();
            WaitUtils.SkipFramesAndInvokeTask(1, _waitTaskCancellation.Token, BroadcastEnemyDeath).Forget();
        }

        void BroadcastEnemyDeath() {
            //Note: This lines ensures explosion when enemy is killed.
            GameplayEvents.BroadcastOnExplosiveObjectHit(transform.position);
            GameplayEvents.BroadcastOnEnemyDeath(this);
        }

        void SetEnemyVisibility(bool isEnabled) {
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