using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    [System.Serializable]
    public class SIDroppedBonuses : SerializableDictionary<BonusType, SIBonus> { }

    public class SIBonusDropController : MonoBehaviour
    {
        [SerializeField] SIDroppedBonuses _droppedBonuses = new SIDroppedBonuses();

        public void TryToRequestBonusDrop()
        {
            SIBonusesEvents.BroadcastOnBonusDropRequested(this);
        }

        public void TryToDropSelectedBonusType(BonusType bonusType)
        {
            //to do zamieniac bonusy wzgledem poziomow / przeladowywać ich parametry
            if (bonusType == BonusType.Undefined)
                return;

            _droppedBonuses[bonusType].MoveObject();
        }
    }
}