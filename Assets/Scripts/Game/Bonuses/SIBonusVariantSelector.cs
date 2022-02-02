using UnityEngine;

namespace SpaceInvaders {
    public class SIBonusVariantSelector : MonoBehaviour {
        [SerializeField] SIBonusDictionary _bonusesVariants;
        BonusType _bonusType;

        public void UseVariant(BonusType bonusType) {
            //todo
            _bonusType = bonusType;
            // _currentVariantSettings = _bonusesVariants[_bonusType].scriptableBonus.bonusSettings;
            // _currentBonusVariantRenderer = _bonusesVariants[_bonusType].bonusRenderer;
        }
    }
}