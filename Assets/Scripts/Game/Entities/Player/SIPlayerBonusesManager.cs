using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders {
    public class SIPlayerBonusesManager : MonoBehaviour {
        Dictionary<BonusType, RuntimeBonus> _activeBonuses;

        void Start() {
            Initialise();
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
        
        void ManageCollectedBonus(BonusSettings collectedBonusSettings) {
            BonusType bonusType = collectedBonusSettings.bonusType;

            if (!IsBonusActive(bonusType))
                _activeBonuses.Add(bonusType, new RuntimeBonus(collectedBonusSettings));
            else
                StopCoroutine(_activeBonuses[collectedBonusSettings.bonusType].bonusRoutine);

            _activeBonuses[collectedBonusSettings.bonusType].bonusRoutine = StartCoroutine(RunBonusRoutine(collectedBonusSettings));
        }

        bool IsBonusActive(BonusType bonusType) {
            return _activeBonuses.ContainsKey(bonusType);
        }

        IEnumerator RunBonusRoutine(BonusSettings bonusSettings) {
            yield return WaitUtils.WaitSecondsAndRunSequence(
                ()=> SIBonusesEvents.BroadcastOnBonusEnabled(bonusSettings),
                () => SIBonusesEvents.BroadcastOnBonusDisabled(bonusSettings),
                bonusSettings.durationTime);
        }
    }
}