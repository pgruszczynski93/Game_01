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

        private Vector2 _raycastOffset;

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
            _raycastOffset = new Vector2(0.0f, 0.25f);
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
            Vector2 raycastPosition = transform.position;
            raycastPosition += _raycastOffset;

            RaycastHit2D raycastHit2D =
                Physics2D.Raycast(raycastPosition, Vector2.up, _raycastDistance, _collisionMask);

            SIEnemyShootBehaviour enemyShotBehaviour = gameObject.GetComponent<SIEnemyShootBehaviour>();

            if (enemyShotBehaviour == null)
            {
                Debug.LogError("Enemy hasn't attached SIEnemyShootBehaviour");
                return null;
            }

            if (raycastHit2D.collider == null)
            {
                return new SIShootedEnemyInfo()
                {
                    currentShootableEnemy = enemyShotBehaviour,
                    nextShootableEnemy = null
                };
            }

            return new SIShootedEnemyInfo()
            {
                currentShootableEnemy = enemyShotBehaviour,
                nextShootableEnemy = raycastHit2D.transform.parent.gameObject.GetComponent<SIEnemyShootBehaviour>()
            };
        }

        public void Respawn()
        {
            IsEnemyDead = false;
            EnableEnemyVisibility(true);
        }

        private void Debug_Respawn()
        {
            Respawn();
        }
    }
}
