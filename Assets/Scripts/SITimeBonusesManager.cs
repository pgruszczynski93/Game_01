using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SITimeBonusesManager : MonoBehaviour
    {
        [SerializeField] private Dictionary<BonusType, SIBonusInfo> _activeTimeDrivenBonuses;

        private SIUIManager _uiManager;

        private void Start()
        {
            SetInitialReferences();
        }

        private void SetInitialReferences()
        {
            _uiManager = SIUIManager.Instance;
            _activeTimeDrivenBonuses = new Dictionary<BonusType, SIBonusInfo>();
        }

        private void Debug_ShowCurrentBonuses()
        {
            SIHelpers.SISimpleLogger(this, "Active bonuses: ", SimpleLoggerTypes.Log);

            foreach (var q in _activeTimeDrivenBonuses)
            {
                SIHelpers.SISimpleLogger(this, "<color=green> Bonus: </color>" + q.Key + " value: " + q.Value.bonusStatistics, SimpleLoggerTypes.Log);
                if (IsBonusWeapon(q.Value.bonusType))
                {
                    SIHelpers.SISimpleLogger(this, "Collected weapon time: "+q.Value.bonusStatistics.gainedWeaponType, SimpleLoggerTypes.Log);
                }
            }
        }

        private bool IsBonusWeapon(BonusType bonusType)
        {
            return bonusType == BonusType.Weapon2x || bonusType == BonusType.Weapon3x;
        }



        public void ManageTimeScheduledBonuses(SIBonusInfo bonusInfo)
        {
            if (_activeTimeDrivenBonuses == null)
            {
                SIHelpers.SISimpleLogger(this, "Can't manage time schedluled bonuses.", SimpleLoggerTypes.Error);
                return;
            }

            SIHelpers.SISimpleLogger(this, "<color=blue> Bonus Parsing </color>", SimpleLoggerTypes.Log);

            if (bonusInfo.bonusStatistics.durationTime > 0)
            {
                TryToAddBonus(bonusInfo);
                Debug_ShowCurrentBonuses();
            }
        }

        private void InitializeBonuses()
        {
            //StopAllCoroutines();
            foreach (KeyValuePair<BonusType, SIBonusInfo> pair in _activeTimeDrivenBonuses)
            {
                StartCoroutine(RunBonusLifecycleRoutine(pair.Value));
            }
        }

        private IEnumerator RunBonusLifecycleRoutine(SIBonusInfo bonusInfo)
        {
            SIHelpers.SISimpleLogger(this, "Finishing bonus lifecycle "+bonusInfo.bonusType, SimpleLoggerTypes.Log);
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


        private void TryToAddBonus(SIBonusInfo bonusInfo)
        {
            SIHelpers.SISimpleLogger(this, "Try to add bonus " , SimpleLoggerTypes.Log);

            switch (bonusInfo.bonusType)
            {
                case BonusType.Shield:
                    CompareBonus(bonusInfo.bonusType, bonusInfo.bonusStatistics.durationTime, bonusInfo);
                    break;
                case BonusType.Weapon2x:
                case BonusType.Weapon3x:
                    CompareBonus(bonusInfo.bonusType, (float) bonusInfo.bonusStatistics.gainedWeaponType, bonusInfo);
                    break;
                default:
                    break;
            }
        }
         
        private void CompareBonus(BonusType dictionaryKey, float compareValue, SIBonusInfo bonusInfo)
        {
            if (_activeTimeDrivenBonuses.ContainsKey(dictionaryKey) == false)
            {
                SIHelpers.SISimpleLogger(this, "<color=blue> Bonus added to dictionary </color>", SimpleLoggerTypes.Log);
                _activeTimeDrivenBonuses.Add(dictionaryKey, bonusInfo);
            }

            float currentDictionaryValue = GetActiveBonusComparatorValue(dictionaryKey);

            SIHelpers.SISimpleLogger(this,"Compare v " + compareValue + " DICT VALUE " + currentDictionaryValue, SimpleLoggerTypes.Log);


            //dodac usuwanie korutyny aktywnego bonusa - np dla broni roznego typu tj jak mamy bron 2 a zlapiemy 3, przerwij 2 wstaw 3 i odpal
            if (compareValue >= currentDictionaryValue)  //remove later equails sign
            {
                _activeTimeDrivenBonuses[dictionaryKey] = bonusInfo;
                StopCoroutine(RunBonusLifecycleRoutine(bonusInfo));
                InitializeBonuses();
            }
        }

        private float GetActiveBonusComparatorValue(BonusType dictionaryKey)
        {
            return dictionaryKey == BonusType.Shield ? _activeTimeDrivenBonuses[dictionaryKey].bonusStatistics.durationTime :
                  (float) _activeTimeDrivenBonuses[dictionaryKey].bonusStatistics.gainedWeaponType;
        }

        private void ResetAppliedBonus(BonusType bonusType)
        {
            SIHelpers.SISimpleLogger(this, "End of bonus " + bonusType, SimpleLoggerTypes.Log);
            _activeTimeDrivenBonuses.Remove(bonusType);
            Debug_ShowCurrentBonuses();
        }
    }
}