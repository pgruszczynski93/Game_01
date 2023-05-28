using System;
using System.Threading;
using PG.Game.Configs;
using Cysharp.Threading.Tasks;
using PG.Game.Systems;
using UnityEngine;

namespace PG.Game.Features.Shield {
    public class ShieldBehaviour : BonusDrivenBehaviour {
        [SerializeField] ShieldSettings _shieldSettings;
        [SerializeField] ShieldAnimatorController _animatorController;

        CancellationTokenSource _shieldCancellation;

        protected override void EnableEnergyBoostForBonus(bool isEnabled) {
            base.EnableEnergyBoostForBonus(isEnabled);
            _animatorController.EnableExtraEnergyAnimation(isEnabled);
        }

        protected override void ManageEnabledBonus() {
            EnableShield();
        }

        protected override void ManageDisabledBonus() {
            DisableShield();
        }

        void EnableShield() {
            EnableRootObject();
            _animatorController.SetShowAnimation();
        }

        void DisableShield() {
            RefreshCancellation();
            DisableShieldTask().Forget();
        }

        async UniTaskVoid DisableShieldTask() {
            try {
                await WaitUtils.StartWaitSecFinishTask(_animatorController.SetHideAnimation,
                    DisableRootObject, _shieldSettings.waitForDisableTime, _shieldCancellation.Token);
            }
            catch (OperationCanceledException) { }
        }

        void RefreshCancellation() {
            _shieldCancellation?.Cancel();
            _shieldCancellation?.Dispose();
            _shieldCancellation = new CancellationTokenSource();
        }
    }
}