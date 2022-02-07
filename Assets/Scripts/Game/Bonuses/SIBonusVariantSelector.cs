using UnityEngine;

namespace SpaceInvaders {
    public class SIBonusVariantSelector : MonoBehaviour {
        [SerializeField] SIBonusDictionary _bonusesVariants;

        Renderer _lastVariantRenderer;
        Renderer _currentVariantRenderer;
        BonusSettings _currentVariantSettings;

        public Renderer CurrentVariantRenderer => _currentVariantRenderer;
        public BonusSettings BonusVariantSettings => _currentVariantSettings;
        
        public void UseVariant(BonusType bonusType) {
            _currentVariantSettings = _bonusesVariants[bonusType].scriptableBonus.bonusSettings;
            _currentVariantRenderer = _bonusesVariants[bonusType].bonusRenderer;
        }
        
        public void TryUpdateBonusVariant(bool isEnabled) {
            if (_currentVariantRenderer == null)
                return;

            if (_lastVariantRenderer != null) {
                _lastVariantRenderer.enabled = false;
            }

            _currentVariantRenderer.enabled = isEnabled;
            _lastVariantRenderer = _currentVariantRenderer;
        }
    }
}