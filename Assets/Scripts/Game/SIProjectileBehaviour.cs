using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIProjectileBehaviour : MonoBehaviour, IProjectile
    {
        [SerializeField] private bool _isMoving;
        [Range(0.01f, 20f),SerializeField] private float _forceScaleFactor;
        [SerializeField] private Rigidbody _rigidbody;

        [SerializeField] private Transform _parentTransform;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private BoxCollider _projectileCollider;

        private Camera _mainCamera;
        private Vector3 _moveForce;
        private Vector3 _parentRelativeLocalPos;
        private Transform _cachedTransform;
        private WaitUntil _waitForProjectileReset;

        public bool IsMoving  { get => _isMoving; set => _isMoving = value; }
        public WaitUntil WaitForProjectileReset  { get => _waitForProjectileReset; set => _waitForProjectileReset = value; }
        public MeshRenderer ProjectileMeshRenderer { get => _meshRenderer; set => _meshRenderer = value; }
        

        private void Start()
        {
            Initialise();
        }

        private void OnEnable()
        {
            AssignEvents();
        }

        private void AssignEvents()
        {
            SIEventsHandler.OnUpdate += CheckIsProjectileOnScreen;
            SIEventsHandler.OnWaveEnd += StopAndResetProjectile;
        }

        private void OnDisable()
        {
            RemoveEvents();
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnUpdate -= CheckIsProjectileOnScreen;
            SIEventsHandler.OnWaveEnd -= StopAndResetProjectile;
        }

        private void Initialise()
        {
            _isMoving = false;
            _waitForProjectileReset = new WaitUntil(() => _isMoving == false);
            _mainCamera = SIGameMasterBehaviour.Instance.MainCamera;
            _moveForce = _parentTransform.up.normalized;
            _cachedTransform = transform;
            _parentRelativeLocalPos = _cachedTransform.localPosition;
            _meshRenderer.enabled = false;
        }

        public void MoveProjectile()
        {
            if (_rigidbody == null || _isMoving)
                return;

            enabled = true;
            TryToEnableParticles(true);
            
            _isMoving = true;
            _meshRenderer.enabled = true;
            _projectileCollider.enabled = true;
            
            _cachedTransform.parent = null;
            _rigidbody.AddForce(_moveForce * _forceScaleFactor, ForceMode.Impulse);
        }
        
        private void StopAndResetProjectile()
        {
            TryToEnableParticles(false);
            _isMoving = false;
            _meshRenderer.enabled = false;
            _projectileCollider.enabled = false;

            _cachedTransform.parent = _parentTransform;
            _cachedTransform.localPosition = _parentRelativeLocalPos;
            
            _rigidbody.velocity = SIHelpers.VectorZero;
            _rigidbody.angularVelocity = SIHelpers.VectorZero;

        }

        private void CheckIsProjectileOnScreen()
        {
            Vector3 projectileViewportPosition = 
                _mainCamera.WorldToViewportPoint(_cachedTransform.localPosition);

            bool isOutOfVerticalBounds = projectileViewportPosition.IsObjectOutOfVerticalViewportBounds3D();

            if (!isOutOfVerticalBounds) 
                return;
            StopAndResetProjectile();
        }

        public void HandleProjectileHit(MonoBehaviour collisionBehaviour = null)
        {
            StopAndResetProjectile();
            enabled = false;
        }
        
        public void HandleWaitOnProjectileReset(MonoBehaviour collisionBehaviour = null)
        {
            StartCoroutine(WaitForResetPossibilityRoutine());
        }

        private IEnumerator WaitForResetPossibilityRoutine()
        {
            yield return _waitForProjectileReset;
            StopAndResetProjectile();
            enabled = false;
        }

        private void TryToEnableParticles(bool canEnableParticles)
        {
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
    }

}

