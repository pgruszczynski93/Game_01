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
                RunBonus(bonusSettings);
                return;
            }

            BonusProperties existingBonusProperties = _activeBonusesLookup[bonusSettings.bonusType];
            BonusProperties collectedBonusProperties = bonusSettings.bonusProperties;

            if (existingBonusProperties.isBonusActive && collectedBonusProperties.bonusLevel > existingBonusProperties.bonusLevel)
            {
                StopCoroutine(existingBonusProperties.bonusRoutine);
                _activeBonusesLookup[bonusSettings.bonusType] = collectedBonusProperties;
                RunBonus(bonusSettings);
            }
            else if (!existingBonusProperties.isBonusActive)
            {
                _activeBonusesLookup[bonusSettings.bonusType] = collectedBonusProperties;
                RunBonus(bonusSettings);
            }

            Debug_PrintActiveBonuses();
        }

        void RunBonus(BonusSettings bonusSettings)
        {
            _activeBonusesLookup[bonusSettings.bonusType].bonusRoutine =
                StartCoroutine(BonusRunner.RunBonus(bonusSettings));
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