
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class SIEnemyProjectilesPool : SIProjectilesPool {

        bool _isPoolReleasingProjectiles;

        protected override void Initialise() {
            base.Initialise();
            //Note: Only to testing
            StartCoroutine(TierTester());
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

        //TESTING METHODS - remove them later
        [Button]
        void TestWeaponTierUpdate(int availableProjectiles) {
            SIGameplayEvents.BroadcastOnEnemyProjectilesCountChanged(availableProjectiles);
        }

        IEnumerator TierTester() {
            while (true) {
                yield return WaitUtils.WaitForCachedSeconds(3f);
                //Note: 1-4 because of array indexing => 0-3
                TestWeaponTierUpdate(Random.Range(1, 4));
            }
        }

        //
    }
}