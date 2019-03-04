using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIUIManager : SIGenericSingleton<SIUIManager>
    {

        [SerializeField] private SIBonusSlotBehaviour[] _availableBonusSlots;

        [SerializeField] private Dictionary<BonusType, SIBonusSlotBehaviour> _bonusUISlots;

        public Dictionary<BonusType, SIBonusSlotBehaviour> BonusUISlots
        {
            get => _bonusUISlots;
            set => _bonusUISlots = value;
        }

        protected override void Awake()
        {
            base.Awake();
            
            SetInitialReferences();
        }

        private void SetInitialReferences()
        {
            _bonusUISlots = new Dictionary<BonusType, SIBonusSlotBehaviour>()
            {
                {BonusType.Life,  _availableBonusSlots[0]},
                {BonusType.Shield,  _availableBonusSlots[1]},
                {BonusType.Weapon,  _availableBonusSlots[2]},
            };
        }
    }
}