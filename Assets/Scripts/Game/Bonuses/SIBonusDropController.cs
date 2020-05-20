using UnityEngine;

namespace SpaceInvaders
{
    public class SIBonusDropController : MonoBehaviour
    {
        [SerializeField] SIBonus _bonusBehaviour;
        [SerializeField] BonusType _selectedBonusType;

        public void TryToRequestBonusDrop()
        {
            SIBonusesEvents.BroadcastOnBonusDropRequested(this);
        }

        public void SetSelectedBonusType(BonusType bonusType)
        {
            if (bonusType == BonusType.Undefined)
                return;

                    //todo: dodać release bonuso
            _selectedBonusType = bonusType;
            Debug.Log(this + " drop " + bonusType);
        }
    }
}