using System;
using System.Collections.Generic;
using MindwalkerStudio.InspectorTools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceInvaders
{
    public class SIBonusSelectorSystem : MonoBehaviour
    {
        [Range(0, 100), SerializeField] int _bonusDropThreshold;
        [SerializeField] BonusSetup[] _bonusesSetup;

        int _dropChancesCount;
        BonusDropInfo[] _dropChanceRates;

        void Initialise()
        {
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
            TryToDropBonus();
        }

        void TryToDropBonus()
        {
            int currentDropPossibility = Random.Range(0, 100);
            if (currentDropPossibility > _bonusDropThreshold)
                return;

            TryToGetBonusesToDrop();
        }

        void SetDropChanceRates()
        {
            _dropChancesCount = _bonusesSetup.Length;
            _dropChanceRates = new BonusDropInfo[_dropChancesCount];
            
            for (int i = 0; i < _dropChancesCount; i++)
            {
                _dropChanceRates[i] = _bonusesSetup[i].bonusSettings.bonusDropInfo;
            }
        }
        void TryToGetBonusesToDrop()
        {
            BonusDropInfo currentDropInfo;
            int currentDropRate = Random.Range(0, 100);
            int bonusesSelectionRange = 0;
            int currentBonusRangeDiffrence;
            List<int> bonusRanges = new List<int> {0};

            for (int i = 0; i < _dropChancesCount; i++)
            {
                currentDropInfo = _dropChanceRates[i];
                if (!IsInBonusRange(currentDropRate, currentDropInfo.minDropRate, currentDropInfo.maxDropRate))
                    return;
                currentBonusRangeDiffrence = currentDropInfo.maxDropRate - currentDropInfo.minDropRate;
                bonusesSelectionRange += currentBonusRangeDiffrence;
                bonusRanges.Add(currentBonusRangeDiffrence);
            }

            float randomizedBonus = Random.Range(0f, 1f);
//            for (int i = 0; i < bonusRanges.Count - 1; i++)
//            {
//                if (randomizedBonus >= bonusRanges[i] && randomizedBonus <= bonusRanges[i + 1])
//                {
//                    Debug.Log("Dropie bonus z przedzialu "+bonusRanges[i]+" "+bonusRanges[i+1]+" RB "+randomizedBonus);
//                }
//            }
            
            //poprawić onenemydeath o info, który enemy jest zabijany
            // poprawić tak zeby ta metoda zwracala tez info o bonusie, który ma zostac upuszcony
        }
        
        bool IsInBonusRange(int dropRate, int minBonusDropRate, int maxBonusDropRate)
        {
            return dropRate >= minBonusDropRate && dropRate <= maxBonusDropRate;
        }

        [Button("Drop Bonus")]
        public void DropBonus()
        {
            Debug.Log("Sorted bonuses");
//            //TO DEBUG ONLY
//            SIEventsHandler.BroadcastOnEnemyDeath();

        }
    }
}