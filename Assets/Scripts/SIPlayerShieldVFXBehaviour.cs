using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerShieldVFXBehaviour : SIVFXManager
    {
        [SerializeField] private GameObject _shield;

        protected override void OnEnable()
        {
            onVFXEnabled += EnableFVX;
        }

        protected override void OnDisable()
        {
            onVFXEnabled += EnableFVX;
        }

        protected override void EnableFVX(bool canBeEnabled)
        {
            if (_shield == null)
            {
                SIHelpers.SISimpleLogger(this, "Shield is not assigned.", SimpleLoggerTypes.Error);
                return;
            }

            if (_shield.activeSelf && canBeEnabled == false)
            {
                _shield.SetActive(false);
                SIHelpers.SISimpleLogger(this, "Shield disabled ", SimpleLoggerTypes.Log);
                return;
            }
            _shield.SetActive(canBeEnabled);
        }
    }
}