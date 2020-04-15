using UnityEngine;

namespace SpaceInvaders
{
    public class SIWeaponBehaviour : MonoBehaviour
    {
        [SerializeField] SIWeaponEntity[] _weaponEntities;

        int _weaponEntitiesCount;

        void Initialise()
        {
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