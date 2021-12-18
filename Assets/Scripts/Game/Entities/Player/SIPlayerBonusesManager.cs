using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders {
    public class SIPlayerBonusesManager : MonoBehaviour {
        static Dictionary<BonusType, RuntimeBonus> _activeBonuses;

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
            BonusType bonusType = collectedBonusSettings.bonusType;

            if (!IsBonusTypeAdded(bonusType))
                _activeBonuses.Add(bonusType, new RuntimeBonus(collectedBonusSettings));
            else
                StopCoroutine(_activeBonuses[collectedBonusSettings.bonusType].bonusRoutine);
            
            _activeBonuses[collectedBonusSettings.bonusType].bonusRoutine = StartCoroutine(RunBonusRoutine(collectedBonusSettings));
        }

        void TryClearCoroutines() {
            if (_activeBonuses == null)
                return;
            Coroutine currentRoutine;
            foreach (KeyValuePair<BonusType, RuntimeBonus> kvp in _activeBonuses) {
                currentRoutine = kvp.Value.bonusRoutine;
                if(currentRoutine != null)
                    StopCoroutine(currentRoutine);
            }
        }

        static bool IsBonusTypeAdded(BonusType bonusType) {
            return _activeBonuses.ContainsKey(bonusType);
        }

        IEnumerator RunBonusRoutine(BonusSettings bonusSettings) {
            _activeBonuses[bonusSettings.bonusType].isCoroutineActive = true;

            yield return WaitUtils.WaitSecondsAndRunSequence(
                () => SIBonusesEvents.BroadcastOnBonusEnabled(bonusSettings), 
                () => SIBonusesEvents.BroadcastOnBonusDisabled(bonusSettings),
                bonusSettings.durationTime);
            
            _activeBonuses[bonusSettings.bonusType].isCoroutineActive = false;
        }
    }
}