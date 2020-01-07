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
//            for (float i = bonusInfo.bonusStatistics.durationTime; i >= 0; i -= 0.25f)
//            {
//                Debug.Log("I : " + i + " "  + bonusInfo.bonusStatistics.durationTime);
//                yield return new WaitForSeconds(0.25f);
//            }
//            yield return SIWaitUtils.WaitForCachedSeconds(bonusInfo.bonusStatistics.durationTime);
            SIUIManager.Instance.BonusUISlots[bonusInfo.bonusType].EnableBonusSlot(false);
            bonusInfo.OnBonusFinishEvent?.Invoke();
            ResetAppliedBonus(bonusInfo.bonusType);
            yield return null;
        }

        public void ManageTimeScheduledBonuses(SIBonusInfo bonusInfo)
        {
//            if (bonusInfo.bonusStatistics.durationTime <= 0) 
//                return;
            
            TryToAddBonus(bonusInfo);
        }


        private void TryToAddBonus(SIBonusInfo bonusInfo)
        {
            switch (bonusInfo.bonusType)
            {
                case BonusType.Shield:
                    CompareBonus(bonusInfo.bonusType, bonusInfo);
                    break;
                case BonusType.Weapon:
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
            if (_activeTimeDrivenBonuses.ContainsKey(dictionaryKey) == false)
            {
                _activeTimeDrivenBonuses.Add(dictionaryKey, bonusInfo);
                _bonusCoroutines[dictionaryKey] = StartCoroutine(RunBonusLifecycleRoutine(bonusInfo));
            }
            else
            {
                TryToUpdateBonuses(dictionaryKey, bonusInfo);
            }
        }

        
        private void TryToUpdateBonuses(BonusType dictionaryKey, SIBonusInfo bonusInfo)
        {
//            int newBonusLevel = (int)bonusInfo.bonusStatistics.gainedCollectibleLevel;
            int newBonusLevel = 0;
            int currentCachedBonusLevel = GetActiveBonusComparableValue(dictionaryKey);

            if (newBonusLevel < currentCachedBonusLevel)
            {
                return;
            }
            
            SIHelpers.SISimpleLogger(this,"Compare v " + newBonusLevel + " DICT VALUE " + currentCachedBonusLevel, SimpleLoggerTypes.Log);

            if (_bonusCoroutines[dictionaryKey] != null)
            {
                StopCoroutine(_bonusCoroutines[dictionaryKey]);
            }
            _bonusCoroutines[dictionaryKey] = StartCoroutine(RunBonusLifecycleRoutine(bonusInfo));

        }

        private int GetActiveBonusComparableValue(BonusType dictionaryKey)
        {
            return 0;
//            return (int)_activeTimeDrivenBonuses[dictionaryKey].bonusStatistics.gainedCollectibleLevel;
        }

        private void ResetAppliedBonus(BonusType bonusType)
        {
            _activeTimeDrivenBonuses.Remove(bonusType);
        }
    }
}