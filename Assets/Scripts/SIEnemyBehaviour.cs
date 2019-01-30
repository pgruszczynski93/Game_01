using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private float _raycastDistance;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private LayerMask _collisionMask;
        private Vector2 _raycastOffset;

        public bool IsEnemyDead { get; set; } = false;

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        private void Start()
        {
            SetInitialReferences();
        }

        private void SetInitialReferences()
        {
            _raycastOffset = new Vector2(0.0f, 0.25f);
        }

        public void Death()
        {
            if (IsEnemyDead == false)
            {
                SIShootedEnemyInfo nextShootableEnemyInfo = ShootAbleEnemy();

                SIEventsHandler.OnSwitchShootableEnemy?.Invoke(nextShootableEnemyInfo);

                IsEnemyDead = true;
                gameObject.SetActive(false);
            }
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
    }
}
