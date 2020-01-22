using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIWeaponBehaviour : MonoBehaviour
    {
        [SerializeField] SIWeaponEntity[] _weaponEntities;

        bool _initialised;
        int _weaponEntitiesCount;

        void Initialise()
        {
            if (_initialised)
                return;
            _initialised = true;
            _weaponEntitiesCount = _weaponEntities.Length;
        }

        void Start()
        {
            Initialise();
        }

        public void TryToLaunchWeaponEntities()
        {
            for (int i = 0; i < _weaponEntitiesCount; i++)
            {
                _weaponEntities[i].TryToLaunchWeapon();
            }
        }
    }
}