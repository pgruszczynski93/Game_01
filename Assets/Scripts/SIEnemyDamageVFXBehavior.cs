using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyDamageVFXBehavior : SIVFXManager
    {
        [SerializeField] private GameObject _damageVFX;

        protected override void OnEnable()
        {
            onVFXEnabled += EnableDamageVFX;
        }

        protected override void OnDisable()
        {
            onVFXEnabled += EnableDamageVFX;
        }

        private void EnableDamageVFX(bool canBeEnabled)
        {
            if (_damageVFX == null)
            {
                SIHelpers.SISimpleLogger(this, "Damage VFX is not assigned.", SimpleLoggerTypes.Error);
                return;
            }

            if (_damageVFX.activeSelf && canBeEnabled == false)
            {
                _damageVFX.SetActive(false);
                SIHelpers.SISimpleLogger(this, "Damage VFX disabled ", SimpleLoggerTypes.Log);
                return;
            }
            _damageVFX.SetActive(canBeEnabled);
        }
    }
}