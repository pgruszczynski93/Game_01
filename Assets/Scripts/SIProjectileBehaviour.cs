using UnityEngine;

namespace SpaceInvaders
{
    public class SIProjectileBehaviour : MonoBehaviour, IMoveable
    {
        [SerializeField] private bool _isMoving;
        [Range(1f,20f)][SerializeField] private float _forceScaleFactor;
        [SerializeField] private Rigidbody _rigidbody;

        [SerializeField] private Transform _cachedParentTransform;
        [SerializeField] private Transform _cachedProjectileTransform;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private BoxCollider _projectileCollider;

        private Camera _mainCamera;
        private Vector3 _moveForce;
        private Vector3 _parentResetPosition;

        public bool IsMoving { get => _isMoving; set => _isMoving = value; }

        private void Awake()
        {
            SetInitialReferences();
        }

        private void OnEnable()
        {
            ResetProjectile();
            SIEventsHandler.OnObjectMovement += CheckIsProjectileOnScreen;
        }

        private void OnDisable()
        {
            ResetProjectile();
            SIEventsHandler.OnObjectMovement -= CheckIsProjectileOnScreen;
        }

        private void SetInitialReferences()
        {
            if (_cachedParentTransform == null)
            {
                return;
            }

            _mainCamera = SIGameMasterBehaviour.Instance.MainCamera;
            _moveForce = _cachedParentTransform.up.normalized;
            _parentResetPosition = new Vector2(0,0);
            _meshRenderer.enabled = false;
        }

        public void MoveObj()
        {
            Move();
        }

        private void CheckIsProjectileOnScreen()
        {
            Vector3 projectileViewportPosition = 
                _mainCamera.WorldToViewportPoint(_cachedProjectileTransform.localPosition);
            if (projectileViewportPosition.IsObjectInScreenVerticalBounds3D())
            {
                ResetProjectile();
            }
        }

        private void Move()
        {
            if (_rigidbody == null)
            {
                return;
            }

            if (_isMoving == false)
            {
                EnableParticles(true);
                _projectileCollider.enabled = true;
                _isMoving = true;
                _meshRenderer.enabled = true;
                _cachedProjectileTransform.parent = null;
                _rigidbody.AddForce(_moveForce.normalized * _forceScaleFactor, ForceMode.Impulse);
            }
        }

        public void OnCollisionResetProjectile(MonoBehaviour collisionBehaviour = null)
        {
            ResetProjectile();
        }

        public void ResetProjectile()
        {
            if(_cachedProjectileTransform == null || _cachedParentTransform == null || _projectileCollider == null)
            {
                SIHelpers.SISimpleLogger(this, "Assign proper values.", SimpleLoggerTypes.Error);
                return;
            }

            SIHelpers.SISimpleLogger(this, "Reset projectile.", SimpleLoggerTypes.Log);

            EnableParticles(false);
            _projectileCollider.enabled = false;
            _isMoving = false;
            _meshRenderer.enabled = false;
            _rigidbody.velocity = new Vector2(0,0);
            _cachedProjectileTransform.parent = _cachedParentTransform;
            _cachedProjectileTransform.localPosition = _parentResetPosition;
            //gameObject.SetActive(false);
        }

        private void EnableParticles(bool canEnableParcles)
        {
            if (_particles == null)
            {
                return;
            }

            _particles.gameObject.SetActive(canEnableParcles);

            if (canEnableParcles)
            {
                _particles.Play();
            }
            else
            {
                _particles.Stop();
            }
        }
    }

}

