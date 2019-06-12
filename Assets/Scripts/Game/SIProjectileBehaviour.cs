using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIProjectileBehaviour : MonoBehaviour, IMoveable
    {
        [SerializeField] private bool _isMoving;
        [Range( 0.01f,20f)][SerializeField] private float _forceScaleFactor;
        [SerializeField] private Rigidbody _rigidbody;

        [SerializeField] private Transform _cachedParentTransform;
        [SerializeField] private Transform _cachedProjectileTransform;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private BoxCollider _projectileCollider;

        private Camera _mainCamera;
        private Vector3 _moveForce;
        private Vector3 _parentResetPosition;
        private WaitUntil _waitUnitil;

        public bool IsMoving { get => _isMoving; set => _isMoving = value; }

        private void Awake()
        {
            Initialize();
        }

        private void OnEnable()
        {
            ResetProjectile();
            AssignEvents();
        }

        private void AssignEvents()
        {
            SIEventsHandler.OnObjectsMovement += CheckIsProjectileOnScreen;
        }

        private void OnDisable()
        {
            ResetProjectile();
            RemoveEvents();
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnObjectsMovement -= CheckIsProjectileOnScreen;
        }

        private void Initialize()
        {
            if (_cachedParentTransform == null)
            {
                return;
            }
            
            _waitUnitil = new WaitUntil(() => _isMoving == false);
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
            if (projectileViewportPosition.IsObjectOutOfViewportVerticalBounds3D())
            {
                ResetProjectile();
            }
        }

        private void Move()
        {
            if (_rigidbody == null || _isMoving)
            {
                return;
            }

            EnableParticles(true);
            _projectileCollider.enabled = true;
            _isMoving = true;
            _meshRenderer.enabled = true;
            _cachedProjectileTransform.parent = null;
            _rigidbody.AddForce(_moveForce.normalized * _forceScaleFactor, ForceMode.Impulse);
        }

        public void OnEnemyDeathResetProjectile(MonoBehaviour collisionBehaviour = null)
        {
            StartCoroutine(WaitForOutOfScreenRoutine());
            ResetProjectile();
        }
        
        //split it to two classes playerprojectile & enemies projectile - more extensibility
        public void OnPlayerProjectileHitsEnemy(MonoBehaviour collisionBehaviour = null)
        {
            ResetProjectile();
        }

        private IEnumerator WaitForOutOfScreenRoutine()
        {
            yield return _waitUnitil;
        }

        public void ResetProjectile()
        {
            SIHelpers.SISimpleLogger(this, "Reset projectile.", SimpleLoggerTypes.Log);

            EnableParticles(false);
            //_projectileCollider.enabled = false;
            _isMoving = false;
            _meshRenderer.enabled = false;
            _rigidbody.velocity = new Vector2(0,0);
            _cachedProjectileTransform.parent = _cachedParentTransform;
            _cachedProjectileTransform.localPosition = _parentResetPosition;
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

        public void StopObj()
        {
            throw new System.NotImplementedException();
        }
    }

}

