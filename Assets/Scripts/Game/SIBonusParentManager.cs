using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIBonusParentManager : MonoBehaviour
    {
        [SerializeField] private int _droppedBonusIndex;
        [SerializeField] private float _bonusDropTreshold;
        // in the future laser weapons will be added;
        [SerializeField] private GameObject _droppedBonus;
        [SerializeField] private GameObject[] _availableBonuses;

        private Dictionary<BonusType, float> _bonusDropChances;
        private Dictionary<CollectibleLevel, float> _weaponDropChances;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_availableBonuses == null)
            {
                SIHelpers.SISimpleLogger(this, "SetInitialReferences - missing references", SimpleLoggerTypes.Error);
                return;
            }

            //temporary values
            _bonusDropChances = new Dictionary<BonusType, float>()
            {
                {BonusType.Life, 0.65f},
                {BonusType.Shield, 0.75f},
                {BonusType.Weapon, 0.85f},
//                {BonusType.Weapon2x, 0.85f},
//                {BonusType.Weapon3x, 0.9f},
            };

            //_weaponDropChances = new Dictionary<WeaponType, float>()
            //{
            //    //{WeaponType.Laser, 0.5f },
            //    //{WeaponType.Laser2x, 0.6f },
            //    //{WeaponType.Laser3x, 0.65f },
            //    {WeaponType.Projectile2x, 0.7f},
            //    {WeaponType.Projectile3x, 0.85f},
            //};
        }

        public void DropBonus()
        {
            float currentDropChance = Random.Range(0.0f, 1.0f);
            if (currentDropChance < _bonusDropTreshold)
            {
                SIHelpers.SISimpleLogger(this, "DropBonus() - no bonus dropped", SimpleLoggerTypes.Log);
                return;
            }

            if (currentDropChance >= _bonusDropChances[BonusType.Life] && currentDropChance < _bonusDropChances[BonusType.Shield])
            {
                SIHelpers.SISimpleLogger(this, "DropBonus() - life bonus dropped", SimpleLoggerTypes.Log);
                _droppedBonusIndex = 0;
            }
            else if (currentDropChance >= _bonusDropChances[BonusType.Shield] &&
                     currentDropChance < _bonusDropChances[BonusType.Weapon])
            {
                SIHelpers.SISimpleLogger(this, "DropBonus() - shield bonus dropped", SimpleLoggerTypes.Log);
                _droppedBonusIndex = 1;
            }
            else
            {
                SIHelpers.SISimpleLogger(this, "DropBonus() - weapon bonus dropped", SimpleLoggerTypes.Log);
//                _droppedBonusIndex = (currentDropChance < _bonusDropChances[BonusType.Weapon3x]) ? 2 : 3;
                _droppedBonusIndex = 2;
            }


            _droppedBonus = _availableBonuses[_droppedBonusIndex];
            _droppedBonus.SetActive(true);
            _droppedBonus.GetComponent<SIBonus>().MoveObject();
        }

        public void ResetBonus()
        {
            if (_droppedBonus == null)
            {
                return;
            }
            _droppedBonusIndex = 0;
        }
    }
}