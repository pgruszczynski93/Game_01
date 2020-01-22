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
            SIBonus.OnBonusEnabled += EnableShield;
        }

        void RemoveEvents()
        {
            SIBonus.OnBonusEnabled -= EnableShield;
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