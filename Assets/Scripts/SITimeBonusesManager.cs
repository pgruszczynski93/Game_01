using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SITimeBonusesManager : MonoBehaviour
    {
        [SerializeField] private Dictionary<BonusType, float> _activeTimeDrivenBonuses;
        [SerializeField] private List<IEnumerator> _activeBonuses;

        private void Start()
        {
            SetInitialReferences();
        }

        private void SetInitialReferences()
        {
            _activeTimeDrivenBonuses = new Dictionary<BonusType, float>()
            {
                {BonusType.Shield, 0.0f},
                {BonusType.Weapon, 1.0f}
            };
            _activeBonuses = new List<IEnumerator>();
        }


        private void Debug_ShowCurrentQueue()
        {
            SIHelpers.SISimpleLogger(this, "Active bonuses: ", SimpleLoggerTypes.Log);

            foreach (var q in _activeTimeDrivenBonuses)
            {
                SIHelpers.SISimpleLogger(this, "Bonus: " + q.Key + " value: " + q.Value, SimpleLoggerTypes.Log);
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
                Debug_ShowCurrentQueue();
            }
        }

        private void TryToAddBonus(SIBonusInfo bonusInfo)
        {
            switch (bonusInfo.bonusType)
            {
                case BonusType.Shield:
                    CompareBonus(bonusInfo.bonusType, bonusInfo.bonusStatistics.durationTime);
                    break;
                case BonusType.Weapon:
                    CompareBonus(bonusInfo.bonusType, (float) bonusInfo.bonusStatistics.gainedWeaponType);
                    break;
                default:
                    break;
            }
        }
         
        private void CompareBonus(BonusType dictionaryKey, float compareValue)
        {
            float dictValue = _activeTimeDrivenBonuses[dictionaryKey];
            Debug.Log("D" + dictValue + " N " + compareValue);
            if (compareValue > dictValue)
            {
                Debug.Log("dupa");
                _activeTimeDrivenBonuses[dictionaryKey] = compareValue;
                SIHelpers.SISimpleLogger(this, "Modification of "+dictionaryKey+" to "+compareValue , SimpleLoggerTypes.Log);
            }
        }
    }
}