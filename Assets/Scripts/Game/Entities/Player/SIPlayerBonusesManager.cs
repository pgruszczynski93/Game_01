using System;
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

        void HandleOnBonusCollected(BonusSettings bonusSettings)
        {
            ManageCollectedBonus(bonusSettings);
        }

        void ManageCollectedBonus(BonusSettings bonusSettings)
        {
            TryToRunCollectedBonus(bonusSettings);
        }

        void TryToRunCollectedBonus(BonusSettings bonusSettings)
        {
            if (!_activeBonusesLookup.ContainsKey(bonusSettings.bonusType))
            {
                _activeBonusesLookup.Add(bonusSettings.bonusType, bonusSettings.bonusProperties);
                RunBonus(bonusSettings, bonusSettings.bonusProperties);
                return;
            }

            TryToUpdateOrRunExistingBonus(bonusSettings);
        }

        void TryToUpdateOrRunExistingBonus(BonusSettings bonusSettings)
        {
            BonusProperties existingBonusProperties = _activeBonusesLookup[bonusSettings.bonusType];
            BonusProperties collectedBonusProperties = bonusSettings.bonusProperties;

            if (existingBonusProperties.isBonusActive &&
                collectedBonusProperties.bonusLevel > existingBonusProperties.bonusLevel)
            {
                StopCoroutine(existingBonusProperties.bonusRoutine);
                RunBonus(bonusSettings, collectedBonusProperties);
            }
            else if (!existingBonusProperties.isBonusActive)
            {
                RunBonus(bonusSettings, collectedBonusProperties);
            }
        }

        void RunBonus(BonusSettings bonusSettings, BonusProperties newBonusProperties )
        {
            _activeBonusesLookup[bonusSettings.bonusType] = newBonusProperties;
            _activeBonusesLookup[bonusSettings.bonusType].bonusRoutine =
                StartCoroutine(RunBonusRoutine(bonusSettings));
        }
        
        IEnumerator RunBonusRoutine(BonusSettings bonusSettings)
        {
            bonusSettings.bonusProperties.isBonusActive = true;
            SIBonusesEvents.BroadcastOnBonusEnabled(bonusSettings.bonusType);
            yield return SIWaitUtils.WaitForCachedSeconds(bonusSettings.bonusProperties.durationTime);
            SIBonusesEvents.BroadcastOnBonusDisabled(bonusSettings.bonusType);
            bonusSettings.bonusProperties.isBonusActive = false;
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