﻿using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using PG.Game.Configs;
using PG.Game.EventSystem;
using PG.Game.Helpers;
using PG.Game.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PG.Game.Entities.Player {
    public class PlayerBonusesManager : MonoBehaviour {
        static Dictionary<BonusType, RuntimeBonus> _activeBonuses;

        [Header("List of bonus settings to use in tests - don't change order!")] [SerializeField]
        List<ScriptableBonus> _scriptableBonuses;

        void Start() {
            Initialise();
        }

        void OnEnable() {
            SubscribeEvents();
        }

        void OnDisable() {
            UnsubscribeEvents();
        }

        void OnDestroy() {
            TryClearCoroutines();
        }

        void Initialise() {
            _activeBonuses = new Dictionary<BonusType, RuntimeBonus>();
        }

        void SubscribeEvents() {
            BonusesEvents.OnBonusCollected += HandleOnBonusCollected;
        }

        void UnsubscribeEvents() {
            BonusesEvents.OnBonusCollected -= HandleOnBonusCollected;
        }

        void HandleOnBonusCollected(BonusSettings collectedBonusSettings) {
            ManageCollectedBonus(collectedBonusSettings);
        }

        public static bool IsBonusActive(BonusType type) {
            return IsBonusTypeAdded(type) && _activeBonuses[type].isBonusTaskActive;
        }

        void ManageCollectedBonus(BonusSettings collectedBonusSettings) {
            TryRunBonusTask(collectedBonusSettings);
        }

        void TryRunBonusTask(BonusSettings collectedBonusSettings) {
            BonusType bonusType = collectedBonusSettings.bonusType;

            if (!IsBonusTypeAdded(bonusType)) {
                _activeBonuses.Add(bonusType, new RuntimeBonus(collectedBonusSettings));
            }
            else {
                CancellationTokenSource cancellation = _activeBonuses[collectedBonusSettings.bonusType].bonusCancellation;
                cancellation?.Cancel();
                cancellation?.Dispose();
                _activeBonuses[collectedBonusSettings.bonusType].bonusCancellation = new CancellationTokenSource();
            }

            _activeBonuses[collectedBonusSettings.bonusType].bonusTask = RunBonusTask(collectedBonusSettings);
        }

        void TryClearCoroutines() {
            if (_activeBonuses == null)
                return;

            foreach (KeyValuePair<BonusType, RuntimeBonus> kvp in _activeBonuses) {
                CancellationTokenSource cancellation = kvp.Value.bonusCancellation;
                cancellation?.Cancel();
                cancellation?.Dispose();
                kvp.Value.bonusCancellation = new CancellationTokenSource();
            }
        }

        static bool IsBonusTypeAdded(BonusType bonusType) {
            return _activeBonuses != null && _activeBonuses.ContainsKey(bonusType);
        }

        async UniTask RunBonusTask(BonusSettings bonusSettings) {
            try {
                RuntimeBonus runtimeBonus = _activeBonuses[bonusSettings.bonusType];
                runtimeBonus.isBonusTaskActive = true;

                await WaitUtils.StartWaitSecFinishTask(
                    () => BonusesEvents.BroadcastOnBonusEnabled(bonusSettings),
                    () => BonusesEvents.BroadcastOnBonusDisabled(bonusSettings),
                    bonusSettings.durationTime, runtimeBonus.bonusCancellation.Token);

                _activeBonuses[bonusSettings.bonusType].isBonusTaskActive = false;
            }
            catch (OperationCanceledException) { }
        }

        [Button]
        void TestHealthBonus() {
            BonusSettings settings = _scriptableBonuses[0].bonusSettings;
            TryRunBonusTask(settings);
        }

        [Button]
        void TestProjectileBonus() {
            BonusSettings settings = _scriptableBonuses[1].bonusSettings;
            TryRunBonusTask(settings);
        }

        [Button]
        void TestShieldSystemBonus() {
            BonusSettings settings = _scriptableBonuses[2].bonusSettings;
            TryRunBonusTask(settings);
        }

        [Button]
        void TestLaserBeamBonus() {
            BonusSettings settings = _scriptableBonuses[3].bonusSettings;
            TryRunBonusTask(settings);
        }

        [Button]
        void TestEnergyBoostBonus() {
            BonusSettings settings = _scriptableBonuses[4].bonusSettings;
            TryRunBonusTask(settings);
        }

        [Button]
        void TestTimeModSlowAllBonus() {
            BonusSettings settings = _scriptableBonuses[5].bonusSettings;
            TryRunBonusTask(settings);
        }

        [Button]
        void TestTimeModFastAllBonus() {
            BonusSettings settings = _scriptableBonuses[6].bonusSettings;
            TryRunBonusTask(settings);
        }
    }
}