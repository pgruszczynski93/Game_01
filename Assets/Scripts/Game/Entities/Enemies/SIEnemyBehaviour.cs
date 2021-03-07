using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] MeshRenderer _meshRenderer;
        [SerializeField] GameObject _colliderParent;
        [SerializeField] SIEnemyShootBehaviour _shootBehaviour;
        [SerializeField] SIEnemyHealth _enemyHealth;

        public SIEnemyShootBehaviour ShootBehaviour
        {
            get => _shootBehaviour;
            set => _shootBehaviour = value;
        }

        void OnEnable() => SubscribeEvents();
        void OnDisable() =>UnsubscribeEvents();

        public void UpdateShootBehaviourSetup(SIShootBehaviourSetup loadedSetup)
        {
            _shootBehaviour.ShootBehaviourSetup = loadedSetup;
        }

        public bool IsEnemyAlive()
        {
            return _enemyHealth.IsAlive();
        }

        void SubscribeEvents()
        {
            SIEnemyGridEvents.OnGridReset += HandleOnGridReset;
            SIGameplayEvents.OnDamage += HandleOnDamage;
        }

        void UnsubscribeEvents()
        {
            SIEnemyGridEvents.OnGridReset -= HandleOnGridReset;
            SIGameplayEvents.OnDamage -= HandleOnDamage;
        }

        void HandleOnGridReset()
        {
            SetEnemyVisibility(true);
            _enemyHealth.SetMaxHealth();
        }

        void HandleOnDamage(DamageInfo damageInfo)
        {
            if (this != damageInfo.ObjectToDamage)
                return;

            _enemyHealth.ApplyDamage(damageInfo.Damage);
            TryToBroadcastEnemyDeathAndSelectNextShootingEnemy();
        }
        
        void TryToBroadcastEnemyDeathAndSelectNextShootingEnemy()
        {
            if (IsEnemyAlive())
                return;

            if (_shootBehaviour.CanShoot)
                _shootBehaviour.TryToSelectNextShootingBehaviour();
            
            // _dropController.TryToRequestBonusDrop();
            SetEnemyVisibility(false);
            StartCoroutine(SIWaitUtils.SkipFramesAndInvoke(1, BroadcastEnemyDeath));
        }

        void BroadcastEnemyDeath()
        {
            SIGameplayEvents.BroadcastOnEnemyDeath(this);
        }
        
        void SetEnemyVisibility(bool isEnabled)
        {
            _colliderParent.SetActive(isEnabled);
            _meshRenderer.enabled = isEnabled;
        }
    }
}