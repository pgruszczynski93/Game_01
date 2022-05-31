using System;
using System.Threading;
using Configs;
using Cysharp.Threading.Tasks;
using SpaceInvaders;
using UnityEngine;

namespace Game.Features.Shield {
    public class SIShieldBehaviour : SIBonusDrivenBehaviour{

        [SerializeField] ShieldSettings _shieldSettings;
        [SerializeField] SIShieldAnimatorController _animatorController;

        CancellationTokenSource _cancellationTokenSource;

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
                await WaitForUtils.StartWaitSecFinishTask(_animatorController.SetHideAnimation,
                    DisableRootObject, _shieldSettings.waitForDisableTime, _cancellationTokenSource.Token);
            }
            catch (OperationCanceledException) { }
        }
        
        void RefreshCancellation() {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }
}