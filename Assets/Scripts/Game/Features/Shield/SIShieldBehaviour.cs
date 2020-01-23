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
            SIPlayerBonusesManager.OnBonusEnabled += HandleOnBonusEnabled;
            SIPlayerBonusesManager.OnBonusDisabled += HandleOnBonusDisabled;
        }

        void RemoveEvents()
        {
            SIPlayerBonusesManager.OnBonusEnabled -= HandleOnBonusEnabled;
            SIPlayerBonusesManager.OnBonusDisabled -= HandleOnBonusDisabled;
        }
        
        void HandleOnBonusEnabled(BonusType bonusType)
        {
            if (bonusType != BonusType.Shield)
                return;
            
            EnableShield();            
        }

        void HandleOnBonusDisabled(BonusType bonusType)
        {
            if (bonusType != BonusType.Shield)
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