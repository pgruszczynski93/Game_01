using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SITimeBonusesManager : MonoBehaviour
    {
//        [SerializeField] private Dictionary<BonusType, SIBonusInfo> _activeTimeDrivenBonuses;
        [SerializeField] private Dictionary<BonusType, Coroutine> _bonusCoroutines;

        private SIUIManager _uiManager;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _uiManager = SIUIManager.Instance;
//            _activeTimeDrivenBonuses = new Dictionary<BonusType, SIBonusInfo>();
            _bonusCoroutines = new Dictionary<BonusType, Coroutine>
            {
                {BonusType.Life, null},
                {BonusType.Weapon, null},
                {BonusType.Shield, null},
            };
        }
    }

}