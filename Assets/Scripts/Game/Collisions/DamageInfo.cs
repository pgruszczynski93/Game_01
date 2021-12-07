using UnityEngine;

namespace SpaceInvaders
{
    [System.Serializable]
    public class DamageInfo
    {
        float _damage;
        MonoBehaviour _objectToDamage;

        public float Damage
        {
            get => _damage;
            set => _damage = value;
        }

        public MonoBehaviour ObjectToDamage
        {
            get => _objectToDamage;
            set => _objectToDamage = value;
        }

        public DamageInfo(float damage)
        {
            _damage = damage;
            _objectToDamage = null;
        }

        public DamageInfo(float damage, MonoBehaviour objectToDamage)
        {
            _damage = damage;
            _objectToDamage = objectToDamage;
        }

        public void SetDamage(float newDamage) {
            _damage = newDamage;
        }

    }
}