using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] SIStatistics _enemyStatistics;
        [SerializeField] MeshRenderer _meshRenderer;
        [SerializeField] GameObject _colliderParent;

//        [SerializeField] private SIVFXManager _destroyVFX;
//        [SerializeField] SIBonusSelectorSystem bonusSelectorSystem;
//        [SerializeField] SIWeaponEntity weaponEntity;

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            _enemyStatistics.isAlive = true;
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
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnWaveEnd -= Respawn;
            SIEventsHandler.OnDebugInputHandling -= Debug_Respawn;
            SIEventsHandler.OnEnemyDeath -= HandleOnEnemyDeath;
        }
        
        public bool IsEnemyAlive()
        {
            return _enemyStatistics.isAlive;
        }

        void HandleOnEnemyDeath()
        {
//            EnableEnemyVisibility(false);
//            _enemyStatistics.isAlive = false;

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
            _enemyStatistics.isAlive = true;
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