using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyBehaviour : MonoBehaviour
    {
        public int enemyIndex;
        
        [SerializeField] private SIStatistics _enemyStatistics;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private GameObject _colliderParent;
//        [SerializeField] private SIVFXManager _destroyVFX;
        [SerializeField] private SIBonusSelector bonusSelector;
        [SerializeField] private SIWeaponEntity weaponEntity;
        [SerializeField] private SIEnemyShootBehaviour _shootBehaviour;

        private void OnEnable()
        {
            AssignEvents();
        }

        public SIEnemyShootBehaviour ShootBehaviour
        {
            get => _shootBehaviour;
        }
        
        private void AssignEvents()
        {
            SIEventsHandler.OnWaveEnd += Respawn;
            SIEventsHandler.OnDebugInputHandling += Debug_Respawn;
        }

        private void OnDisable()
        {
            RemoveEvents();
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnWaveEnd += Respawn;
            SIEventsHandler.OnDebugInputHandling -= Debug_Respawn;
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _enemyStatistics.isAlive = true;
        }

        public bool IsEnemyAlive()
        {
            return _enemyStatistics.isAlive;
        }

        public void Death(MonoBehaviour collisionBehaviour = null)
        {
            if (_enemyStatistics.isAlive == false)
            {
                return;
            } 
            
            EnableEnemyVisibility(false);
//            playerBonusManager.DropBonus();
            _enemyStatistics.isAlive = false;
            weaponEntity.HandleWaitOnProjectileReset();

            SIEventsHandler.BroadcastOnShootingEnemiesUpdate(enemyIndex);
        }

        private void EnableEnemyVisibility(bool canEnable)
        {
            _colliderParent.SetActive(canEnable);
            _meshRenderer.enabled = canEnable;
//            _destroyVFX.TryToEnableAndDetachVFX(canEnable == false);
        }

        public void Spawn()
        {
            
        }
        
        public void Respawn()
        {
            _enemyStatistics.isAlive = true;
            //weaponEntity.enabled = true;
            EnableEnemyVisibility(true);
        }

        private void Debug_Respawn()
        {
            if (!Input.GetKeyDown(KeyCode.L))
            {
                return;
            }
            SIHelpers.SISimpleLogger(this, "Debug_Respawn()", SimpleLoggerTypes.Log);
            Respawn();
        }
    }
}
