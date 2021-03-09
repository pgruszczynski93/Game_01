using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIBonusManager : MonoBehaviour {

        [SerializeField] Queue<SIBonus> _availableBonuses;

        void Start() => Initialise();
        void Initialise() {
            
        }
        
        void OnEnable() => SubscribeEvents(); 
        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents() {
            SIGameplayEvents.OnEnemyDeath += HandleOnEnemyDeath;
        }
        
        void UnsubscribeEvents()
        {
            SIGameplayEvents.OnEnemyDeath -= HandleOnEnemyDeath;
        }

        void HandleOnEnemyDeath(SIEnemyBehaviour enemy) {
            var enemyWorldPos = enemy.transform.position;
            
        }
    }
    

    // [Serializable]
    // public struct BonusRangeInfo
    // {
    //     public int bonusDropChanceDiffrence;
    //     public BonusType bonusType;
    // } 
    // public class SIBonusSelectorSystem : MonoBehaviour
    // {
    //     [Range(0, 99), SerializeField] int _bonusDropThreshold;
    //     [SerializeField] BonusSetup[] _bonusesSetup;
    //
    //     int _dropChancesCount;
    //     int _globalMinDropRate;
    //     int _globalMaxDropRate;
    //     int _currentSelectionRange;
    //     SIBonusDropController _requestingBonusDropController;
    //     BonusDropInfo[] _dropChanceRates;
    //     List<BonusRangeInfo> _availableBonuses;
    //
    //     void Initialise()
    //     {
    //         _globalMaxDropRate = -1;
    //         _globalMinDropRate = int.MaxValue;
    //         _availableBonuses = new List<BonusRangeInfo>();
    //         SetDropChanceRates();
    //     }
    //
    //     void Start()
    //     {
    //         Initialise();
    //     }
    //
    //     void OnEnable()
    //     {
    //         AssignEvents();
    //     }
    //
    //     void OnDisable()
    //     {
    //         RemoveEvents();
    //     }
    //     
    //     void AssignEvents()
    //     {
    //         SIBonusesEvents.OnBonusDropRequested += HandleOnBonusDropRequested;
    //     }
    //
    //     void RemoveEvents()
    //     {
    //         SIBonusesEvents.OnBonusDropRequested -= HandleOnBonusDropRequested;
    //     }
    //
    //     void HandleOnBonusDropRequested(SIBonusDropController objSender)
    //     {
    //         _availableBonuses.Clear();
    //         _currentSelectionRange = 0;
    //         _requestingBonusDropController = objSender;
    //         _requestingBonusDropController.TryToDropSelectedBonusType(TryToSelectProperBonusType());
    //     }
    //
    //     BonusType TryToSelectProperBonusType()
    //     {
    //         int currentDropPossibility = Random.Range(0, 100);
    //         if (currentDropPossibility > _bonusDropThreshold)
    //             return BonusType.Undefined;
    //
    //         TryToSetAvailableBonuses();
    //         return SelectFinalBonusType();
    //     }
    //
    //     void SetDropChanceRates()
    //     {
    //         _dropChancesCount = _bonusesSetup.Length;
    //         _dropChanceRates = new BonusDropInfo[_dropChancesCount];
    //
    //         BonusDropInfo currentDropInfo;
    //         
    //         for (int i = 0; i < _dropChancesCount; i++)
    //         {
    //             currentDropInfo = _bonusesSetup[i].bonusSettings.bonusDropInfo;
    //             _dropChanceRates[i] = currentDropInfo;
    //             if (currentDropInfo.minDropRate < _globalMinDropRate)
    //                 _globalMinDropRate = currentDropInfo.minDropRate;
    //             if (currentDropInfo.maxDropRate > _globalMaxDropRate)
    //                 _globalMaxDropRate = currentDropInfo.maxDropRate;
    //
    //         }
    //     }
    //     void TryToSetAvailableBonuses()
    //     {
    //         BonusDropInfo currentDropInfo;
    //         int dropScore = Random.Range(_globalMinDropRate, _globalMaxDropRate);
    //
    //         for (int i = 0; i < _dropChancesCount; i++)
    //         {
    //             currentDropInfo = _dropChanceRates[i];
    //             if (!IsDropScoreInBonusRange(dropScore, currentDropInfo.minDropRate, currentDropInfo.maxDropRate))
    //                 return;
    //
    //             BonusRangeInfo bonusInRange = new BonusRangeInfo
    //             {
    //                 bonusType = currentDropInfo.bonusType,
    //                 bonusDropChanceDiffrence = currentDropInfo.maxDropRate - currentDropInfo.minDropRate
    //             };
    //             _currentSelectionRange += bonusInRange.bonusDropChanceDiffrence;
    //             _availableBonuses.Add(bonusInRange);
    //         }
    //         _availableBonuses.Sort((a, b) => b.bonusDropChanceDiffrence.CompareTo(a.bonusDropChanceDiffrence));
    //     }
    //
    //     BonusType SelectFinalBonusType()
    //     {
    //         if (_availableBonuses == null || _availableBonuses.Count == 0)
    //             return BonusType.Undefined;
    //
    //         int availableBonusesCount = _availableBonuses.Count;
    //         int rangeMin = 0;
    //         int rangeMax = _availableBonuses[0].bonusDropChanceDiffrence;
    //         int finalBonusScore = Random.Range(0, _currentSelectionRange);
    //
    //         for (int i = 0; i < availableBonusesCount ; i++)
    //         {
    //             if (finalBonusScore >= rangeMin && finalBonusScore < rangeMax)
    //             {
    //                 return _availableBonuses[i].bonusType;
    //             }
    //
    //             rangeMin = rangeMax;
    //             if(i < availableBonusesCount - 1)
    //                 rangeMax += _availableBonuses[i + 1].bonusDropChanceDiffrence;
    //         }
    //
    //         return _availableBonuses[0].bonusType;
    //     }
    //     
    //     bool IsDropScoreInBonusRange(int dropRate, int minBonusDropRate, int maxBonusDropRate)
    //     {
    //         return dropRate >= minBonusDropRate && dropRate <= maxBonusDropRate;
    //     }
    //
    //     [Button("Drop Bonus")]
    //     public void DropBonus()
    //     {
    //         _globalMaxDropRate = -1;
    //         _currentSelectionRange = 0;
    //         _globalMinDropRate = int.MaxValue;
    //         _availableBonuses = new List<BonusRangeInfo>();
    //         SetDropChanceRates();
    //         Debug.Log(TryToSelectProperBonusType());
    //     }
    // }
}