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
            TryToAddCollectedBonus(bonusSettings);
        }

        void TryToAddCollectedBonus(BonusSettings bonusSettings)
        {
            if (!_activeBonusesLookup.ContainsKey(bonusSettings.bonusType))
            {
                _activeBonusesLookup.Add(bonusSettings.bonusType, bonusSettings.bonusProperties);
            }

            Debug_PrintActiveBonuses();
        }

        void Debug_PrintActiveBonuses()
        {
            foreach (var bonus in _activeBonusesLookup)
            {
                Debug.Log($"Bonus: {bonus.Key}");
            }
        }
    }
}