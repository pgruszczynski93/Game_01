using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] EntitySetup _entitySetup;
        [SerializeField] MeshRenderer _meshRenderer;
        [SerializeField] GameObject _colliderParent;

        EntitySettings _entitySettings;
        SIEnemyStatistics _enemyEntityStatistics;
        
//        [SerializeField] private SIVFXManager _destroyVFX;
//        [SerializeField] SIBonusSelectorSystem bonusSelectorSystem;
//        [SerializeField] SIWeaponEntity weaponEntity;

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            _entitySettings = _entitySetup.entitySettings;
            _enemyEntityStatistics = new SIEnemyStatistics
            {
                isAlive = true, currentHealth = _entitySettings.initialStatistics.currentHealth, enemyLevel = 1
            };
        }

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
            SIEventsHandler.OnWaveEnd += Respawn;
            SIEventsHandler.OnDebugInputHandling += Debug_Respawn;
            SIEventsHandler.OnEnemyDeath += HandleOnEnemyDeath;
            
            SIGameplayEvents.OnDamage += HandleOnDamage;
        }
        void RemoveEvents()
        {
            SIEventsHandler.OnWaveEnd -= Respawn;
            SIEventsHandler.OnDebugInputHandling -= Debug_Respawn;
            SIEventsHandler.OnEnemyDeath -= HandleOnEnemyDeath;
            
            SIGameplayEvents.OnDamage -= HandleOnDamage;
        }

        void HandleOnDamage(DamageInfo damageInfo)
        {
            if (this != damageInfo.ObjectToDamage)
                return;
            
            ApplyDamage(damageInfo.Damage);
        }
        void ApplyDamage(float damage)
        {
            _enemyEntityStatistics.currentHealth -= damage;
            // todo: remove and add eventrs 
            if (_enemyEntityStatistics.currentHealth < 0)
            {
            
                EnableEnemyVisibility(false);
                _enemyEntityStatistics.isAlive = false;
            }
        }
        
        public bool IsEnemyAlive()
        {
            return _enemyEntityStatistics.isAlive;
        }

        void HandleOnEnemyDeath(SIEnemyBehaviour enemyBehaviour)
        {
            if (this != enemyBehaviour)
                return;
            
            EnableEnemyVisibility(false);
            _enemyEntityStatistics.isAlive = false;
        }
        
        public void Death()
        {
//            if (_enemyStatistics.isAlive == false) return;
//
//            EnableEnemyVisibility(false);
//            playerBonusManager.DropBonus();
//            _enemyStatistics.isAlive = false;
//            weaponEntity.HandleWaitOnProjectileReset();
//
//            SIEventsHandler.BroadcastOnShootingEnemiesUpdate(enemyIndex);
        }

        void EnableEnemyVisibility(bool canEnable)
        {
            _colliderParent.SetActive(canEnable);
            _meshRenderer.enabled = canEnable;
//            _destroyVFX.TryToEnableAndDetachVFX(canEnable == false);
        }

        public void Spawn() { }

        public void Respawn()
        {
            _enemyEntityStatistics.isAlive = true;
            //weaponEntity.enabled = true;
            EnableEnemyVisibility(true);
        }

        void Debug_Respawn()
        {
            if (!Input.GetKeyDown(KeyCode.L)) return;

            SIHelpers.SISimpleLogger(this, "Debug_Respawn()", SimpleLoggerTypes.Log);
            Respawn();
        }
    }
}