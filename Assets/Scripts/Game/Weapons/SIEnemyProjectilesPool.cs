using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class SIEnemyProjectilesPool : SIProjectilesPool {

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
            SIEnemyGridEvents.OnShotInvoked += HandleOnShotInvoked;
            SIGameplayEvents.OnEnemyProjectilesCountChanged += HandleOnProjectilesCountChanged;
        }

        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            SIEnemyGridEvents.OnShotInvoked -= HandleOnShotInvoked;
            SIGameplayEvents.OnEnemyProjectilesCountChanged -= HandleOnProjectilesCountChanged;
        }

        void HandleOnShotInvoked(SIEnemyShootController shootController) {
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
            SIGameplayEvents.BroadcastOnEnemyProjectilesCountChanged(availableProjectiles);
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