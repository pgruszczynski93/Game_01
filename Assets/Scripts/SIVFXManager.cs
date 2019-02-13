using UnityEngine;
using System;

namespace SpaceInvaders
{
    public class SIVFXManager : MonoBehaviour
    {
        [SerializeField] protected GameObject _effectVFX;   // to do: more handled effects
        protected Action<bool> onVFXEnabled = delegate { };

        protected virtual void OnEnable()
        {
            onVFXEnabled += EnableFVX;
        }

        protected virtual void OnDisable()
        {
            onVFXEnabled += EnableFVX;
        }

        public virtual void OnEnableVFXCallback(bool canBeEnabled)
        {
            onVFXEnabled?.Invoke(canBeEnabled);
        }

        protected virtual void EnableFVX(bool canBeEnabled)
        {
            if (_effectVFX == null)
            {
                SIHelpers.SISimpleLogger(this, "Effect is not assigned.", SimpleLoggerTypes.Error);
                return;
            }

            if (_effectVFX.activeSelf && canBeEnabled == false)
            {
                _effectVFX.SetActive(false);
                SIHelpers.SISimpleLogger(this, "Effect disabled ", SimpleLoggerTypes.Log);
                return;
            }
            _effectVFX.SetActive(canBeEnabled);
        }
    }
}