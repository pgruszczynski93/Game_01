using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyBehaviour : MonoBehaviour, IRespawnable
    {
        [SerializeField] private float _raycastDistance;
        [SerializeField] private LayerMask _collisionMask;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private BoxCollider[] _colliders;


        private Vector2 _raycastOffset;

        public bool IsEnemyDead { get; set; } = false;

        private void OnEnable()
        {
            SIEventsHandler.OnWaveEnd += Respawn;
        }

        private void OnDisable()
        {
            SIEventsHandler.OnWaveEnd += Respawn;
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

                IsEnemyDead = true;
                EnableEnemyVisibility(false);
            }
        }


        private void EnableEnemyVisibility(bool canEnable)
        {
            if (_colliders == null || _meshRenderer == null)
            {
                return;
            }

            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i].enabled = canEnable;
            }
            _meshRenderer.enabled = canEnable;
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
            EnableEnemyVisibility(true);
        }
    }
}
