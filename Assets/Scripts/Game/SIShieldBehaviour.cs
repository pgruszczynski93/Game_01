using UnityEngine;

namespace SpaceInvaders
{
    public class SIShieldBehaviour : MonoBehaviour
    {
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
            _shieldVfx.TryToManageVFX(true, false, true);
        }
    }
}