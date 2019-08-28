using UnityEngine;
using System;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class SIVFXManager : MonoBehaviour
    {
        [SerializeField] private GameObject _effectVFX;   // to do: more handled effects
        //protected Action<bool> onVFXEnabled = delegate { };
        private Dictionary<VFXActions, Action<bool>> _onVFXEnabled;

        [SerializeField] private Transform _parentTransform;
        [SerializeField] private Transform _effectCachedTransform;

        private void Awake()
        {
            SetInitialReferences();
        }

        private void SetInitialReferences()
        {
            _onVFXEnabled = new Dictionary<VFXActions, Action<bool>>()
            {
                {VFXActions.EnableVFX, delegate { }},
                {VFXActions.EnableAndDetachVFX, delegate { }},
                {VFXActions.EnableAndAttachVFX, delegate { }}
            };

            _effectCachedTransform = _effectVFX.transform;
        }

        private void OnEnable()
        {
            _onVFXEnabled[VFXActions.EnableVFX] += EnableFVX;
            _onVFXEnabled[VFXActions.EnableAndDetachVFX] += EnableAndDetachVFX;
            _onVFXEnabled[VFXActions.EnableAndAttachVFX] += EnableAndAttachVFX;
        }

        private void OnDisable()
        {
            _onVFXEnabled[VFXActions.EnableVFX] -= EnableFVX;
            _onVFXEnabled[VFXActions.EnableAndDetachVFX] -= EnableAndDetachVFX;
            _onVFXEnabled[VFXActions.EnableAndAttachVFX] -= EnableAndAttachVFX;
        }

        private void EnableFVX(bool canBeEnabled)
        {
            if (_effectVFX == null)
            {
                SIHelpers.SISimpleLogger(this, "Effect is not assigned.", SimpleLoggerTypes.Error);
                return;
            }

            if (_effectVFX.activeSelf && canBeEnabled == false)
            {
                _effectVFX.SetActive(false);
                return;
            }
//            SIHelpers.SISimpleLogger(this, "Effect applied ", SimpleLoggerTypes.Log);
            _effectVFX.SetActive(canBeEnabled);
        }

        private void EnableAndDetachVFX(bool canBeEnabled)
        {
            _effectCachedTransform.parent = null;
//            SIHelpers.SISimpleLogger(this, "EnableAndDetachVFX()", SimpleLoggerTypes.Log);

            EnableFVX(canBeEnabled);

            StartCoroutine(SIWaitUtils.WaitAndInvoke(1.5f, ResetEffectParent));
        }

        public void ResetEffectParent()
        {
            _effectCachedTransform.parent = _parentTransform;
//            SIHelpers.SISimpleLogger(this, "<color=red>Reset and parenting to</color> : " + _parentTransform.gameObject.name, SimpleLoggerTypes.Log);
            _effectCachedTransform.localPosition = SIHelpers.VectorZero;
            EnableFVX(false);
        }

        private void EnableAndAttachVFX(bool canBeEnabled)
        {
//            SIHelpers.SISimpleLogger(this, "EnableAndAttachVFX not implemented!", SimpleLoggerTypes.Error);
        }

        public void OnEnableVFXCallback(bool canBeEnabled)
        {
            _onVFXEnabled[VFXActions.EnableVFX]?.Invoke(canBeEnabled);
        }

        public void HandleOnEnableAndDetachVFX(bool canBeEnabled)
        {
            _onVFXEnabled[VFXActions.EnableAndDetachVFX]?.Invoke(canBeEnabled);
        }

        public void OnEnableAndAttachVFXCallback(bool canBeEnabled)
        {
            _onVFXEnabled[VFXActions.EnableAndAttachVFX]?.Invoke(canBeEnabled);
        }

    }
}