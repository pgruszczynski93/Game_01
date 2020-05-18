using System;
using System.Collections.Generic;
using System.Linq;
using MindwalkerStudio.InspectorTools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceInvaders
{
    [Serializable]
    public struct BonusRangeInfo
    {
        public int bonusDropChanceDiffrence;
        public BonusType bonusType;
    } 
    public class SIBonusSelectorSystem : MonoBehaviour
    {
        [Range(0, 99), SerializeField] int _bonusDropThreshold;
        [SerializeField] BonusSetup[] _bonusesSetup;

        int _dropChancesCount;
        int _globalMinDropRate;
        int _globalMaxDropRate;
        int _currentSelectionRange;
        BonusDropInfo[] _dropChanceRates;
        [SerializeField] List<BonusRangeInfo> availableBonuses;

        void Initialise()
        {
            _globalMaxDropRate = -1;
            _globalMinDropRate = int.MaxValue;
            availableBonuses = new List<BonusRangeInfo>();
            SetDropChanceRates();
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
            SIEventsHandler.OnEnemyDeath += HandleOnEnemyDeath;
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnEnemyDeath -= HandleOnEnemyDeath;
        }

        void HandleOnEnemyDeath(SIEnemyBehaviour enemyBehaviours)
        {
            //debug:
            TryToDropBonus();
        }

        void TryToDropBonus()
        {
            int currentDropPossibility = Random.Range(0, 100);
            if (currentDropPossibility > _bonusDropThreshold)
                return;

            TryToSetAvailableBonuses();
            TryToSelectFinalBonus();
        }

        void SetDropChanceRates()
        {
            _dropChancesCount = _bonusesSetup.Length;
            _dropChanceRates = new BonusDropInfo[_dropChancesCount];

            BonusDropInfo currentDropInfo;
            
            for (int i = 0; i < _dropChancesCount; i++)
            {
                currentDropInfo = _bonusesSetup[i].bonusSettings.bonusDropInfo;
                _dropChanceRates[i] = currentDropInfo;
                if (currentDropInfo.minDropRate < _globalMinDropRate)
                    _globalMinDropRate = currentDropInfo.minDropRate;
                if (currentDropInfo.maxDropRate > _globalMaxDropRate)
                    _globalMaxDropRate = currentDropInfo.maxDropRate;

            }
        }
        void TryToSetAvailableBonuses()
        {
            BonusDropInfo currentDropInfo;
            int dropScore = Random.Range(_globalMinDropRate, _globalMaxDropRate);

            for (int i = 0; i < _dropChancesCount; i++)
            {
                currentDropInfo = _dropChanceRates[i];
                if (!IsDropScoreInBonusRange(dropScore, currentDropInfo.minDropRate, currentDropInfo.maxDropRate))
                    return;
    
                BonusRangeInfo bonusInRange = new BonusRangeInfo
                {
                    bonusType = currentDropInfo.bonusType,
                    bonusDropChanceDiffrence = currentDropInfo.maxDropRate - currentDropInfo.minDropRate
                };
                _currentSelectionRange += bonusInRange.bonusDropChanceDiffrence;
                availableBonuses.Add(bonusInRange);
            }
            availableBonuses.Sort((a, b) => b.bonusDropChanceDiffrence.CompareTo(a.bonusDropChanceDiffrence));
        }

        void TryToSelectFinalBonus()
        {
            if (availableBonuses == null || availableBonuses.Count == 0)
                return;

            int availableBonusesCount = availableBonuses.Count;
            int rangeMin = 0;
            int rangeMax = availableBonuses[0].bonusDropChanceDiffrence;
            int finalBonusScore = Random.Range(0, _currentSelectionRange);
            BonusRangeInfo finalBonus = availableBonuses[0];

            for (int i = 0; i < availableBonusesCount ; i++)
            {
                if (finalBonusScore >= rangeMin && finalBonusScore < rangeMax)
                {
                    finalBonus = availableBonuses[i];
                }

                rangeMin = rangeMax;
                if(i < availableBonusesCount - 1)
                    rangeMax += availableBonuses[i + 1].bonusDropChanceDiffrence;
            }
            
            Debug.Log("SEL BON " +finalBonusScore +" "+ finalBonus.bonusType);
        }
        
        bool IsDropScoreInBonusRange(int dropRate, int minBonusDropRate, int maxBonusDropRate)
        {
            return dropRate >= minBonusDropRate && dropRate <= maxBonusDropRate;
        }

        [Button("Drop Bonus")]
        public void DropBonus()
        {
            _globalMaxDropRate = -1;
            _currentSelectionRange = 0;
            _globalMinDropRate = int.MaxValue;
            availableBonuses = new List<BonusRangeInfo>();
            SetDropChanceRates();
            TryToDropBonus();
        }
    }
}