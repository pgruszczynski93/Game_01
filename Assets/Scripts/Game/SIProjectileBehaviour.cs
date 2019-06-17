using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIProjectileBehaviour : MonoBehaviour, IMoveable
    {
        [SerializeField] private bool _isMoving;
        [Range(0.01f, 20f)][SerializeField] private float _forceScaleFactor;
        [SerializeField] private Rigidbody _rigidbody;

        [SerializeField] private Transform _cachedParentTransform;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private BoxCollider _projectileCollider;

        private Camera _mainCamera;
        private Vector3 _moveForce;
        private Vector3 _parentRelativeLocalPos;
        private Transform _cachedTransform;
        private WaitUntil _waitUntil;

        public bool IsMoving { get => _isMoving; set => _isMoving = value; }

        private void Awake()
        {
            Initialise();
        }

        private void OnEnable()
        {
            AssignEvents();
        }

        private void AssignEvents()
        {
            SIEventsHandler.OnObjectsMovement += CheckIsProjectileOnScreen;
            SIEventsHandler.OnWaveEnd += ResetProjectile;
        }

        private void OnDisable()
        {
            RemoveEvents();
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnObjectsMovement -= CheckIsProjectileOnScreen;
            SIEventsHandler.OnWaveEnd -= ResetProjectile;
        }

        private void Initialise()
        {
            if (_cachedParentTransform == null)
            {
                return;
            }
            
            _waitUntil = new WaitUntil(() => _isMoving == false);
            _mainCamera = SIGameMasterBehaviour.Instance.MainCamera;
            _moveForce = _cachedParentTransform.up.normalized;
            _cachedTransform = transform;
            _parentRelativeLocalPos = _cachedTransform.localPosition;
            _meshRenderer.enabled = false;
        }

        public void MoveObj()
        {
            Move();
        }

        private void CheckIsProjectileOnScreen()
        {
            Vector3 projectileViewportPosition = 
                _mainCamera.WorldToViewportPoint(_cachedTransform.localPosition);

            bool isOutOfVerticalBounds = projectileViewportPosition.IsObjectOutOfVerticalViewportBounds3D();
//            Debug.Log(isOutOfVerticalBounds + " " + projectileViewportPosition);
            if (isOutOfVerticalBounds)
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
            _cachedTransform.parent = null;
            _rigidbody.AddForce(_moveForce * _forceScaleFactor, ForceMode.Impulse);
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
            yield return _waitUntil;
        }

        public void ResetProjectile()
        {
            EnableParticles(false);
            _projectileCollider.enabled = false;
            _isMoving = false;
            _meshRenderer.enabled = false;
            _rigidbody.velocity = SIHelpers.VectorZero;
            _rigidbody.angularVelocity = SIHelpers.VectorZero;
            _cachedTransform.parent = _cachedParentTransform;
            _cachedTransform.localPosition = _parentRelativeLocalPos;
        }

        private void EnableParticles(bool canEnableParticles)
        {
            if (_particles == null)
            {
                return;
            }

            _particles.gameObject.SetActive(canEnableParticles);

            if (canEnableParticles)
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

