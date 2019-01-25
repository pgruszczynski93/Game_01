using UnityEngine;

namespace SpaceInvaders
{
    public class SIProjectileBehaviour : MonoBehaviour, IMoveable
    {
        [SerializeField] private bool _isMoving;
        [Range(8f,20f)][SerializeField] private float _forceScaleFactor;
        [SerializeField] private Rigidbody2D _rigidbody2D;

        [SerializeField] private Transform _cachedParentTransform;
        [SerializeField] private Transform _cachedProjectileTransform;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ParticleSystem _particles;

        private Vector2 _moveForce;
        private Vector2 _parentResetPosition;
        
        private void Awake()
        {
            SetInitialReferences();
        }

        private void OnEnable()
        {
            ResetProjectile();
        }

        private void OnDisable()
        {
            ResetProjectile();
        }

        private void SetInitialReferences()
        {
            if (_cachedParentTransform == null)
            {
                return;
            }
            _moveForce = _cachedParentTransform.up.normalized;
            _parentResetPosition = new Vector2(0,0);
            _spriteRenderer.enabled = false;
        }

        public void MoveObj()
        {
            Move();
        }

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.gameObject.CompareTag("Enemy"))
            {
                ResetProjectile();
            }
        }

        private void Move()
        {
            if (_rigidbody2D == null)
            {
                return;
            }

            if (_isMoving == false)
            {
                _isMoving = true;
                _spriteRenderer.enabled = true;
                _cachedProjectileTransform.parent = null;
                _rigidbody2D.AddForce(_moveForce.normalized * _forceScaleFactor, ForceMode2D.Impulse);
                EnableParticles(true);
            }
        }

        private void ResetProjectile()
        {
            if(_cachedProjectileTransform == null || _cachedParentTransform == null)
            {
                return;
            }

            _isMoving = false;
            _spriteRenderer.enabled = false;
            _rigidbody2D.velocity = new Vector2(0,0);
            _cachedProjectileTransform.parent = _cachedParentTransform;
            _cachedProjectileTransform.localPosition = _parentResetPosition;
            EnableParticles(false);
        }

        private void EnableParticles(bool canEnableParcles)
        {
            if (_particles == null)
            {
                return;
            }

            if (canEnableParcles)
            {
                _particles.Play();
            }
            else
            {
                _particles.Pause();
            }
        }
    }

}

