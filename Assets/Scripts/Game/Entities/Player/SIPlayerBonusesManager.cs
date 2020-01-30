using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerBonusesManager : MonoBehaviour
    {
        bool _initialised;
        Dictionary<BonusType, BonusProperties> _activeBonusesLookup;

        void Initialise()
        {
            if (_initialised)
                return;

            _initialised = true;
            _activeBonusesLookup = new Dictionary<BonusType, BonusProperties>();
        }

        void Start()
        {
            Initialise();
        }

        void OnEnable()
        {
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        void AssignEvents()
        {
            SIEventsHandler.OnBonusCollected += HandleOnBonusCollected;
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnBonusCollected -= HandleOnBonusCollected;
        }

        void HandleOnBonusCollected(BonusSettings collectedBonusSettings)
        {
            ManageCollectedBonus(collectedBonusSettings);
        }

        void ManageCollectedBonus(BonusSettings collectedBonusSettings)
        {
            TryToRunCollectedBonus(collectedBonusSettings);
        }

        void TryToRunCollectedBonus(BonusSettings collectedBonusSettings)
        {
            if (!_activeBonusesLookup.ContainsKey(collectedBonusSettings.bonusType))
            {
                _activeBonusesLookup.Add(collectedBonusSettings.bonusType, collectedBonusSettings.bonusProperties);
                RunBonus(collectedBonusSettings);
                return;
            }

            UpdateOrRunExistingBonus(collectedBonusSettings);
        }

        void UpdateOrRunExistingBonus(BonusSettings collectedBonusSettings)
        {
            StopCoroutine(_activeBonusesLookup[collectedBonusSettings.bonusType].bonusRoutine);
            RunBonus(collectedBonusSettings);
        }

        void RunBonus(BonusSettings collectedBonusSettings)
        {
            BonusSettings bonusSettingsCopy = collectedBonusSettings;
            bonusSettingsCopy.bonusProperties.bonusRoutine = StartCoroutine(RunBonusRoutine(collectedBonusSettings));
            _activeBonusesLookup[collectedBonusSettings.bonusType] = bonusSettingsCopy.bonusProperties;
        }
        
        IEnumerator RunBonusRoutine(BonusSettings bonusSettings)
        {
            SIBonusesEvents.BroadcastOnBonusEnabled(bonusSettings);
            yield return SIWaitUtils.WaitForCachedSeconds(bonusSettings.bonusProperties.durationTime);
            SIBonusesEvents.BroadcastOnBonusDisabled(bonusSettings);
        }

        void Debug_PrintActiveBonuses()
        {
            foreach (var bonus in _activeBonusesLookup)
            {
                Debug.Log($"Bonus: {bonus.Key} level: {bonus.Value.bonusLevel}");
            }
        }
    }
}