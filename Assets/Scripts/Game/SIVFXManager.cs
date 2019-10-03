using UnityEngine;
using System;
using System.Collections.Generic;

namespace SpaceInvaders
{
    public class SIVFXManager : MonoBehaviour
    {
        [SerializeField] private GameObject _effectVFX; // to do: more handled effects
        [SerializeField] private Transform _parentTransform;
        
        private Transform _effectTransform;
        void Start()
        {
            Initialise();
        }

        void Initialise()
        {
            _effectTransform = _effectVFX.transform;
        }
        public void TryToEnableVFX(bool canBeEnabled)
        {
            if (_effectVFX == null)
            {
                Debug.LogError("Effect is not assigned.", this);
                return;
            }

            if (_effectVFX.activeSelf && canBeEnabled == false)
            {
                _effectVFX.SetActive(false);
                return;
            }

            _effectVFX.SetActive(canBeEnabled);
        }
        public void TryToEnableAndDetachVFX(bool canBeEnabled)
        {
            _effectTransform.parent = null;
            TryToEnableVFX(canBeEnabled);
            StartCoroutine(routine: SIWaitUtils.WaitAndInvoke(1.5f, ResetEffectParent));
        }
        void ResetEffectParent()
        {
            _effectTransform.parent = _parentTransform;
            _effectTransform.localPosition = SIHelpers.VectorZero;
            TryToEnableVFX(false);
        }
    }
}