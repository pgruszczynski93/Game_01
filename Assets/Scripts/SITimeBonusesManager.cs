using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SITimeBonusesManager : MonoBehaviour
    {
        [SerializeField] private Dictionary<BonusType, SIBonusInfo> _activeTimeDrivenBonuses;
        [SerializeField] private List<BonusType> _editorActiveBonuses_debug;

        private void Start()
        {
            SetInitialReferences();
        }

        private void SetInitialReferences()
        {
            _activeTimeDrivenBonuses = new Dictionary<BonusType, SIBonusInfo>();
            _editorActiveBonuses_debug = new List<BonusType>();
        }


        private void Debug_ShowCurrenBonuses()
        {
            SIHelpers.SISimpleLogger(this, "Active bonuses: ", SimpleLoggerTypes.Log);

            foreach (var q in _activeTimeDrivenBonuses)
            {
                SIHelpers.SISimpleLogger(this, "Bonus: " + q.Key + " value: " + q.Value.bonusStatistics, SimpleLoggerTypes.Log);
            }
        }

        public void ManageTimeScheduledBonuses(SIBonusInfo bonusInfo)
        {
            if (_activeTimeDrivenBonuses == null)
            {
                SIHelpers.SISimpleLogger(this, "Can't manage time schedluled bonuses.", SimpleLoggerTypes.Error);
                return;
            }

            if (bonusInfo.bonusStatistics.durationTime > 0)
            {
                TryToAddBonus(bonusInfo);
                Debug_ShowCurrenBonuses();
            }
        }

        private void InitializeBonuses()
        {
            StopAllCoroutines();
            foreach (KeyValuePair<BonusType, SIBonusInfo> pair in _activeTimeDrivenBonuses)
            {
                StartCoroutine(FinishBonusLifecycleRoutine(pair.Value));
            }
        }

        private IEnumerator FinishBonusLifecycleRoutine(SIBonusInfo bonusInfo)
        {
            SIHelpers.SISimpleLogger(this, "Finishing bonus lifecycle "+bonusInfo.bonusType, SimpleLoggerTypes.Log);
            yield return new WaitForSeconds(bonusInfo.bonusStatistics.durationTime);
            bonusInfo.OnBonusFinishEvent?.Invoke();
            ResetAppliedBonus(bonusInfo.bonusType);
        }

        private void TryToAddBonus(SIBonusInfo bonusInfo)
        {
            switch (bonusInfo.bonusType)
            {
                case BonusType.Shield:
                    CompareBonus(bonusInfo.bonusType, bonusInfo.bonusStatistics.durationTime, bonusInfo);
                    break;
                case BonusType.Weapon:
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
                _activeTimeDrivenBonuses.Add(dictionaryKey, bonusInfo);
            }

            float dictValue = _activeTimeDrivenBonuses[dictionaryKey].bonusStatistics.durationTime;

            if (compareValue >= dictValue)  //remove later equails sign
            {
                _activeTimeDrivenBonuses[dictionaryKey] = bonusInfo;
                _editorActiveBonuses_debug.AddUnique(dictionaryKey);
                SIHelpers.SISimpleLogger(this, "Modification of "+dictionaryKey+" to "+compareValue , SimpleLoggerTypes.Log);
                InitializeBonuses();
            }
        }

        private void ResetAppliedBonus(BonusType bonusType)
        {
            SIHelpers.SISimpleLogger(this, "End of bonus " + bonusType, SimpleLoggerTypes.Log);
            float resetValue = bonusType == BonusType.Shield ? 0.0f : 1.0f;
            _activeTimeDrivenBonuses[bonusType].bonusStatistics.durationTime = resetValue;
        }
    }
}