using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIWeaponEntity : SIEntity
    {
        [SerializeField] SIWeaponSetup _weaponSetup;
        [SerializeField] SIWeaponSettings _weaponSettins;
        [SerializeField] Rigidbody _rigidbody;
        [SerializeField] Transform _parentTransform;
        [SerializeField] ParticleSystem _particles;
        [SerializeField] BoxCollider _weaponCollider;
        [SerializeField] MeshRenderer _meshRenderer;

        bool _initialised;
        bool _isMoving;

        float _topWorldLimit;
        float _bottomWorldLimit;
        Vector3 _moveForce;
        Vector3 _parentRelativeLocalPos;
        Transform _thisTransform;
        WaitUntil _waitForProjectileReset;

        void Initialise()
        {
            if (_initialised)
                return;

            _weaponSettins = _weaponSetup.weaponSettings;
            _initialised = true;
            _isMoving = false;
            _meshRenderer.enabled = false;
            _moveForce = _parentTransform.up.normalized;
            _thisTransform = transform;
            _parentRelativeLocalPos = _thisTransform.localPosition;
            _waitForProjectileReset = new WaitUntil(() => _isMoving == false);

            ScreenEdges screenWorldEdges = SIGameMasterBehaviour.Instance.ScreenAreaCalculator.CalculatedScreenEdges;
            _topWorldLimit = screenWorldEdges.topScreenEdge + _weaponSettins.movementLimitOffset;
            _bottomWorldLimit = screenWorldEdges.bottomScreenEdge - _weaponSettins.movementLimitOffset;
        }

        void Start()
        {
            Initialise();
        }

        void OnEnable()
        {
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        void AssignEvents()
        {
            SIEventsHandler.OnUpdate += CheckIsProjectileOnScreen;
            SIEventsHandler.OnWaveEnd += StopAndResetProjectile;
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnUpdate -= CheckIsProjectileOnScreen;
            SIEventsHandler.OnWaveEnd -= StopAndResetProjectile;
        }

        public void TryToLaunchWeapon()
        {
            if (_isMoving)
                return;

            _isMoving = true;
            _meshRenderer.enabled = true;
            _weaponCollider.enabled = true;
            _thisTransform.parent = null;
            TryToEnableParticles(true);
            _rigidbody.AddForce(_moveForce * _weaponSettins.launchForceMultiplier, ForceMode.Impulse);
        }

        void StopAndResetProjectile()
        {
            TryToEnableParticles(false);
            _isMoving = false;
            _meshRenderer.enabled = false;
            _weaponCollider.enabled = false;

            _thisTransform.parent = _parentTransform;
            _thisTransform.localPosition = _parentRelativeLocalPos;

            _rigidbody.velocity = SIHelpers.VectorZero;
            _rigidbody.angularVelocity = SIHelpers.VectorZero;
        }

        void CheckIsProjectileOnScreen()
        {
            bool isInVerticalSpace =
                SIScreenUtils.IsInVerticalWorldScreenLimit(_thisTransform.position, _bottomWorldLimit, _topWorldLimit);
            if (isInVerticalSpace)
                return;
            StopAndResetProjectile();
        }

        public void HandleProjectileHit(MonoBehaviour collisionBehaviour = null)
        {
            StopAndResetProjectile();
        }

        public void HandleWaitOnProjectileReset(MonoBehaviour collisionBehaviour = null)
        {
            StartCoroutine(WaitForResetPossibilityRoutine());
        }

        IEnumerator WaitForResetPossibilityRoutine()
        {
            yield return _waitForProjectileReset;
            StopAndResetProjectile();
        }

        void TryToEnableParticles(bool canEnableParticles)
        {
            _particles.gameObject.SetActive(canEnableParticles);

            if (canEnableParticles)
                _particles.Play();
            else
                _particles.Stop();
        }
    }
}