using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SITimeBonusesManager : MonoBehaviour
    {
        [SerializeField] private Dictionary<BonusType, SIBonusInfo> _activeTimeDrivenBonuses;
        [SerializeField] private Dictionary<BonusType, Coroutine> _bonusCoroutines;

        private SIUIManager _uiManager;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _uiManager = SIUIManager.Instance;
            _activeTimeDrivenBonuses = new Dictionary<BonusType, SIBonusInfo>();
            _bonusCoroutines = new Dictionary<BonusType, Coroutine>
            {
                {BonusType.Life, null},
                {BonusType.Weapon, null},
                {BonusType.Shield, null},
            };
        }
        
        private IEnumerator RunBonusLifecycleRoutine(SIBonusInfo bonusInfo)
        {
            SIUIManager.Instance.BonusUISlots[bonusInfo.bonusType].EnableBonusSlot(true);
            for (float i = bonusInfo.bonusStatistics.durationTime; i >= 0; i -= 0.25f)
            {
                Debug.Log("I : " + i + " "  + bonusInfo.bonusStatistics.durationTime);
                yield return new WaitForSeconds(0.25f);
            }
            //yield return SIHelpers.CustomDelayRoutine(bonusInfo.bonusStatistics.durationTime);
            SIUIManager.Instance.BonusUISlots[bonusInfo.bonusType].EnableBonusSlot(false);
            bonusInfo.OnBonusFinishEvent?.Invoke();
            ResetAppliedBonus(bonusInfo.bonusType);
        }

        private void Debug_ShowCurrentBonuses()
        {
            SIHelpers.SISimpleLogger(this, "Active bonuses: ", SimpleLoggerTypes.Log);

            foreach (var q in _activeTimeDrivenBonuses)
            {
                SIHelpers.SISimpleLogger(this, "<color=green> Bonus: </color>" + q.Key + " value: " + q.Value.bonusStatistics, SimpleLoggerTypes.Log);
            }
        }

        public void ManageTimeScheduledBonuses(SIBonusInfo bonusInfo)
        {
            SIHelpers.SISimpleLogger(this, "<color=blue> Bonus Parsing </color>", SimpleLoggerTypes.Log);

            if (bonusInfo.bonusStatistics.durationTime <= 0) 
                return;
            
            TryToAddBonus(bonusInfo);
            Debug_ShowCurrentBonuses();
        }


        private void TryToAddBonus(SIBonusInfo bonusInfo)
        {
            SIHelpers.SISimpleLogger(this, "Try to add bonus " , SimpleLoggerTypes.Log);

            switch (bonusInfo.bonusType)
            {
                case BonusType.Shield:
                    SIHelpers.SISimpleLogger(this, "V Shield " , SimpleLoggerTypes.Log);
                    CompareBonus(bonusInfo.bonusType, bonusInfo);
                    break;
                case BonusType.Weapon:
                    SIHelpers.SISimpleLogger(this, "V Weapon " , SimpleLoggerTypes.Log);
                    CompareBonus(bonusInfo.bonusType,  bonusInfo);
                    break;
                case BonusType.Life:
                    break;
                default:
                    break;
            }
        }
         
        private void CompareBonus(BonusType dictionaryKey, SIBonusInfo bonusInfo)
        {
            SIHelpers.SISimpleLogger(this, "V CompareBonus " , SimpleLoggerTypes.Log);

            if (_activeTimeDrivenBonuses.ContainsKey(dictionaryKey) == false)
            {
                SIHelpers.SISimpleLogger(this, "<color=blue> Bonus added to dictionary </color>", SimpleLoggerTypes.Log);
                _activeTimeDrivenBonuses.Add(dictionaryKey, bonusInfo);
            }
            
            Debug.Log("DUPA 1");
            TryToUpdateBonuses(dictionaryKey, bonusInfo);
        }

        
        private void TryToUpdateBonuses(BonusType dictionaryKey, SIBonusInfo bonusInfo)
        {
            int newBonusLevel = (int)bonusInfo.bonusStatistics.gainedCollectibleLevel;
            int currentCachedBonusLevel = GetActiveBonusComparatorValue(dictionaryKey);
            Debug.Log("DUPA 2");

            SIHelpers.SISimpleLogger(this,"Compare v " + newBonusLevel + " DICT VALUE " + currentCachedBonusLevel, SimpleLoggerTypes.Log);

            if (newBonusLevel >= currentCachedBonusLevel)  
            {

                SIHelpers.SISimpleLogger(this, "<color=blue> Breaking coroutine with dictionary update </color>", SimpleLoggerTypes.Log);

                if (_bonusCoroutines[dictionaryKey] != null)
                {
                    StopCoroutine(_bonusCoroutines[dictionaryKey]);
                }
                _bonusCoroutines[dictionaryKey] = StartCoroutine(RunBonusLifecycleRoutine(bonusInfo));
            }

        }

        private int GetActiveBonusComparatorValue(BonusType dictionaryKey)
        {
            return (int)_activeTimeDrivenBonuses[dictionaryKey].bonusStatistics.gainedCollectibleLevel;
        }

        private void ResetAppliedBonus(BonusType bonusType)
        {
            SIHelpers.SISimpleLogger(this, "End of bonus " + bonusType, SimpleLoggerTypes.Log);
            _activeTimeDrivenBonuses.Remove(bonusType);
            Debug_ShowCurrentBonuses();
        }
    }
}