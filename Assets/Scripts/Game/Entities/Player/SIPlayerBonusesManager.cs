using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SpaceInvaders {
    public class SIPlayerBonusesManager : MonoBehaviour {
        static Dictionary<BonusType, RuntimeBonus> _activeBonuses;

        [Header("List of bonus settings to use in tests - don't change order!")]
        [SerializeField] List<ScriptableBonus> _scriptableBonuses;

        CancellationTokenSource _cancellationTokenSource;
        
        void Start() {
            Initialise();
        }

        void OnDestroy() {
            TryClearCoroutines();
        }

        void OnEnable() {
            SubscribeEvents();
        }

        void OnDisable() {
            UnsubscribeEvents();
        }
        
        void Initialise() {
            _activeBonuses = new Dictionary<BonusType, RuntimeBonus>();
        }

        void SubscribeEvents() {
            SIBonusesEvents.OnBonusCollected += HandleOnBonusCollected;
        }

        void UnsubscribeEvents() {
            SIBonusesEvents.OnBonusCollected -= HandleOnBonusCollected;
        }

        void HandleOnBonusCollected(BonusSettings collectedBonusSettings) {
            ManageCollectedBonus(collectedBonusSettings);
        }

        public static bool IsBonusActive(BonusType type) {
            return IsBonusTypeAdded(type) && _activeBonuses[type].isCoroutineActive;
        }
        
        void ManageCollectedBonus(BonusSettings collectedBonusSettings) {
            TryRunBonusRoutine(collectedBonusSettings);
        }

        void TryRunBonusRoutine(BonusSettings collectedBonusSettings) {
            BonusType bonusType = collectedBonusSettings.bonusType;

            if (!IsBonusTypeAdded(bonusType))
                _activeBonuses.Add(bonusType, new RuntimeBonus(collectedBonusSettings));
            else
                StopCoroutine(_activeBonuses[collectedBonusSettings.bonusType].bonusTask);

            _activeBonuses[collectedBonusSettings.bonusType].bonusTask = RunBonusTask(collectedBonusSettings).Forget();
        }

        void TryClearCoroutines() {
            if (_activeBonuses == null)
                return;
            UniTask currentRoutine;
            foreach (KeyValuePair<BonusType, RuntimeBonus> kvp in _activeBonuses) {
                currentRoutine = kvp.Value.bonusTask;
                if(currentRoutine != null)
                    StopCoroutine(currentRoutine);
            }
        }

        static bool IsBonusTypeAdded(BonusType bonusType) {
            return _activeBonuses != null && _activeBonuses.ContainsKey(bonusType);
        }

        async UniTask RunBonusTask(BonusSettings bonusSettings) {
            try {
                _activeBonuses[bonusSettings.bonusType].isCoroutineActive = true;

                await WaitForUtils.StartWaitSecFinishTask(
                    () => SIBonusesEvents.BroadcastOnBonusEnabled(bonusSettings),
                    () => SIBonusesEvents.BroadcastOnBonusDisabled(bonusSettings),
                    bonusSettings.durationTime, _cancellationTokenSource.Token);

                _activeBonuses[bonusSettings.bonusType].isCoroutineActive = false;
            }
            catch (OperationCanceledException) { }
        }

        [Button]
        void TestHealthBonus() {
            BonusSettings settings = _scriptableBonuses[0].bonusSettings;
            TryRunBonusRoutine(settings);
        }
        
        [Button]
        void TestProjectileBonus() {
            BonusSettings settings = _scriptableBonuses[1].bonusSettings;
            TryRunBonusRoutine(settings);
        }
        
        [Button]
        void TestShieldSystemBonus() {
            BonusSettings settings = _scriptableBonuses[2].bonusSettings;
            TryRunBonusRoutine(settings);
        }
                
        [Button]
        void TestLaserBeamBonus() {
            BonusSettings settings = _scriptableBonuses[3].bonusSettings;
            TryRunBonusRoutine(settings);
        }
        
        [Button]
        void TestEnergyBoostBonus() {
            BonusSettings settings = _scriptableBonuses[4].bonusSettings;
            TryRunBonusRoutine(settings);
        }
                
        [Button]
        void TestTimeModSlowAllBonus() {
            BonusSettings settings = _scriptableBonuses[5].bonusSettings;
            TryRunBonusRoutine(settings);
        }
        
        [Button]
        void TestTimeModFastAllBonus() {
            BonusSettings settings = _scriptableBonuses[6].bonusSettings;
            TryRunBonusRoutine(settings);
        }
    }
}