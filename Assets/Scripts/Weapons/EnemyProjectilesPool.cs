using System.Threading;
using Cysharp.Threading.Tasks;
using PG.Game.Entities.Enemies;
using PG.Game.EventSystem;
using PG.Game.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PG.Game.Weapons {
    public class EnemyProjectilesPool : ProjectilesPool {
        bool _isPoolReleasingProjectiles;
        CancellationTokenSource _weaponTierChangeCancellation;

        protected override void Initialise() {
            base.Initialise();
            //Note: Only to testing
            RefreshCancellation();
            ChangeWeaponTierTestTask().Forget();
        }

        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            EnemyGridEvents.OnShotInvoked += HandleOnShotInvoked;
            GameplayEvents.OnEnemyProjectilesCountChanged += HandleOnProjectilesCountChanged;
        }

        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            EnemyGridEvents.OnShotInvoked -= HandleOnShotInvoked;
            GameplayEvents.OnEnemyProjectilesCountChanged -= HandleOnProjectilesCountChanged;
        }

        void HandleOnShotInvoked(EnemyShootController shootController) {
            if (!shootController.CanShoot || _isPoolReleasingProjectiles)
                return;

            _isPoolReleasingProjectiles = true;
            _currentSlotSet = shootController.GetProjectileSlotsParent();
            _currentSlotIndex = 0;
            for (int i = 0; i < _currentSlotSet.Length; i++) {
                SetNextObjectFromPool();
                _currentSlotIndex++;
            }

            _isPoolReleasingProjectiles = false;
        }

        void RefreshCancellation() {
            _weaponTierChangeCancellation?.Cancel();
            _weaponTierChangeCancellation?.Dispose();
            _weaponTierChangeCancellation = new CancellationTokenSource();
        }

        //TESTING METHODS - remove them later
        [Button]
        void TestWeaponTierUpdate(int availableProjectiles) {
            GameplayEvents.BroadcastOnEnemyProjectilesCountChanged(availableProjectiles);
        }

        async UniTaskVoid ChangeWeaponTierTestTask() {
            while (true) {
                await WaitUtils.WaitSecondsAndInvokeTask(3f,
                    () => {
                        //Note: 1-4 because of array indexing => 0-3
                        TestWeaponTierUpdate(Random.Range(1, 4));
                    },
                    _weaponTierChangeCancellation.Token);
            }
            // ReSharper disable once FunctionNeverReturns
        }
    }
}