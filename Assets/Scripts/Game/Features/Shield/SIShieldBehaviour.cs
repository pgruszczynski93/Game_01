using UnityEngine;

namespace SpaceInvaders
{
    public class SIShieldBehaviour : MonoBehaviour
    {
        [SerializeField] GameObject  _rootObject;
        [SerializeField] SIGameObjectVFX _shieldVfx;

        void OnEnable()
        {    
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        void AssignEvents()
        {
            SIBonusesEvents.OnBonusEnabled += HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled += HandleOnBonusDisabled;
        }

        void RemoveEvents()
        {
            SIBonusesEvents.OnBonusEnabled -= HandleOnBonusEnabled;
            SIBonusesEvents.OnBonusDisabled -= HandleOnBonusDisabled;
        }
        
        void HandleOnBonusEnabled(BonusSettings bonusSettings)
        {
            if (bonusSettings.bonusType != BonusType.Shield)
                return;
            
            EnableShield();            
        }

        void HandleOnBonusDisabled(BonusSettings bonusSettings)
        {
            if (bonusSettings.bonusType != BonusType.Shield)
                return;

            //todo::  REMOVE IT LATYEA
            DisableNecessaryObjects();
        }
        
        void EnableShield()
        {
            EnableNecessaryObjects();
            _shieldVfx.TryToManageVFX(true, false, false);
        }

        void EnableNecessaryObjects()
        {
            _rootObject.SetActive(true);
        }
        void DisableNecessaryObjects()
        {
            _rootObject.SetActive(false);
        }
    }
}