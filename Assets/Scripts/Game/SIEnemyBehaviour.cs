using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyBehaviour : MonoBehaviour, IRespawnable
    {
        [SerializeField] private float _raycastDistance;
        [SerializeField] private SIStatistics _enemyStatistics;
        [SerializeField] private LayerMask _collisionMask;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private GameObject _colliderParent;
        [SerializeField] private SIVFXManager _destroyVFX;
        [SerializeField] private SIEnemyMovement _enemyMovement;
        [SerializeField] private SIBonusParentManager _bonusManager;
        [SerializeField] private SIProjectileBehaviour _projectileParent;
        [SerializeField] private SIEnemyShootBehaviour _shootBehaviour;

        private Vector3 _raycastOffset;

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
            _raycastOffset = new Vector3(0.0f, 0.25f);
            _enemyStatistics.isAlive = true;
        }

        public void Death(MonoBehaviour collisionBehaviour = null)
        {
            if (_enemyStatistics.isAlive == false)
            {
                return;
            } 
            
            SIShootedEnemyInfo nextShootableEnemyInfo = NextShootableEnemy();
            
            EnableEnemyVisibility(false);
            _enemyMovement.StopObj();
            _bonusManager.DropBonus();
            _enemyStatistics.isAlive = false;
            _projectileParent.enabled = false;
            
            if (nextShootableEnemyInfo.currentShootableEnemy == null)
            {
                return;
            }
            
            SIEventsHandler.BroadcastOnSwitchShootableEnemy(nextShootableEnemyInfo);

        }

        private void EnableEnemyVisibility(bool canEnable)
        {
            _colliderParent.SetActive(canEnable);
            _meshRenderer.enabled = canEnable;
            _projectileParent.enabled = canEnable;
            _destroyVFX.OnEnableVFXCallback(canEnable == false);
        }

        public SIShootedEnemyInfo NextShootableEnemy()
        {
            Vector3 raycastPosition = transform.position;
            raycastPosition += _raycastOffset;

            RaycastHit raycastHitInfo;
            SIEnemyShootBehaviour nextShootingEnemyBehaviour;

            SIShootedEnemyInfo shootInfo = new SIShootedEnemyInfo()
            {
                currentShootableEnemy = _shootBehaviour
            };

            if (Physics.Raycast(raycastPosition, Vector3.up, out raycastHitInfo, _raycastDistance, _collisionMask))
            {
                if (raycastHitInfo.collider != null && raycastHitInfo.collider.CompareTag(SIStringTags.ENEMY))
                {
                    GameObject shootableGameObject = raycastHitInfo.transform.gameObject;
                    nextShootingEnemyBehaviour = shootableGameObject.GetComponent<SIEnemyColliderBehaviour>().ParentShootBehaviour;

                    if (nextShootingEnemyBehaviour == null)
                    {
                        shootInfo.currentShootableEnemy = null;
                        return shootInfo;
                    }

                    Debug.DrawRay(raycastPosition, Vector3.up, Color.red, 1f);

                    shootInfo.nextShootableEnemy = nextShootingEnemyBehaviour;
                }
            }

            return shootInfo;
        }

        public void Respawn()
        {
            _enemyStatistics.isAlive = true;
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
