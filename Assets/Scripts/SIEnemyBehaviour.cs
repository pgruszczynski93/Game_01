using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyBehaviour : MonoBehaviour, IRespawnable
    {
        [SerializeField] private float _raycastDistance;
        [SerializeField] private LayerMask _collisionMask;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private GameObject _colliderParent;
        [SerializeField] private SIVFXManager _destroyVFX;

        private Vector3 _raycastOffset;

        public bool IsEnemyDead { get; set; } = false;

        private void OnEnable()
        {
            SIEventsHandler.OnWaveEnd += Respawn;
            SIEventsHandler.OnDebugInputHandling += Debug_Respawn;
        }

        private void OnDisable()
        {
            SIEventsHandler.OnWaveEnd += Respawn;
            SIEventsHandler.OnDebugInputHandling -= Debug_Respawn;
        }

        private void Start()
        {
            SetInitialReferences();
        }

        private void SetInitialReferences()
        {
            _raycastOffset = new Vector3(0.0f, 0.25f);
        }

        public void Death(MonoBehaviour collisionBehaviour = null)
        {
            if (IsEnemyDead == false)
            {
                SIShootedEnemyInfo nextShootableEnemyInfo = ShootAbleEnemy();
                SIEventsHandler.OnSwitchShootableEnemy?.Invoke(nextShootableEnemyInfo);
                EnableEnemyVisibility(false);
                IsEnemyDead = true;
            }
        }


        private void EnableEnemyVisibility(bool canEnable)
        {
            if (_colliderParent == null || 
                _meshRenderer == null ||
                _destroyVFX == null)
            {
                return;
            }

            _colliderParent.SetActive(canEnable);
            _meshRenderer.enabled = canEnable;
            _destroyVFX.OnEnableVFXCallback(canEnable == false);
            //gameObject.SetActive(canEnable);
        }

        public SIShootedEnemyInfo ShootAbleEnemy()
        {
            Vector3 raycastPosition = transform.position;
            raycastPosition += _raycastOffset;

            RaycastHit raycastHitInfo;
            SIEnemyShootBehaviour enemyShotBehaviour = gameObject.GetComponent<SIEnemyShootBehaviour>();
            SIEnemyShootBehaviour nextShootingEnemyBehaviour;

            if (enemyShotBehaviour == null)
            {
                SIHelpers.SISimpleLogger(this, "Enemy hasn't attached SIEnemyShootBehaviour", SimpleLoggerTypes.Log);
                return null;
            }

            SIShootedEnemyInfo shootInfo = new SIShootedEnemyInfo()
            {
                currentShootableEnemy = enemyShotBehaviour
            };

            if (Physics.Raycast(raycastPosition, Vector3.up, out raycastHitInfo, _raycastDistance, _collisionMask))
            {
                if (raycastHitInfo.collider != null && raycastHitInfo.collider.CompareTag("Enemy"))
                {
                    nextShootingEnemyBehaviour = raycastHitInfo.transform.parent.parent.parent.gameObject.GetComponent<SIEnemyShootBehaviour>();

                    if (nextShootingEnemyBehaviour == null)
                    {
                        SIHelpers.SISimpleLogger(this, "Next enemy doen's have attached SIEnemyShootBehaviour", SimpleLoggerTypes.Log);
                        shootInfo.currentShootableEnemy = null;
                        return shootInfo;
                    }

                    SIHelpers.SISimpleLogger(this, "<color=blue>Shootable switched </color>" + nextShootingEnemyBehaviour.gameObject.name, SimpleLoggerTypes.Log);
                    Debug.DrawRay(raycastPosition, Vector3.up, Color.red, 1f);

                    shootInfo.nextShootableEnemy = nextShootingEnemyBehaviour;
                }
            }

            return shootInfo;
        }

        public void Respawn()
        {
            IsEnemyDead = false;
            EnableEnemyVisibility(true);
        }

        private void Debug_Respawn()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                SIHelpers.SISimpleLogger(this, "Debug_Respawn()", SimpleLoggerTypes.Log);
                Respawn();
            }
        }
    }
}
