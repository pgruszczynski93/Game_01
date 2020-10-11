using System;
using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIWeaponEntity : SIEntity
    {
        [SerializeField] SIWeaponSetup _weaponSetup;
        [SerializeField] SIWeaponSettings _weaponSettings;
        [SerializeField] Rigidbody _rigidbody;
        [SerializeField] Transform _parentTransform;
        [SerializeField] ParticleSystem _particles;
        [SerializeField] BoxCollider _weaponCollider;
        [SerializeField] MeshRenderer _meshRenderer;

        bool _isMoving;

        float _topWorldLimit;
        float _bottomWorldLimit;
        Vector3 _moveForce;
        Vector3 _parentRelativeLocalPos;
        Vector3 _initialLocalAngles;
        Transform _thisTransform;
        DamageInfo _damageInfo;

        protected override void Initialise()
        {
            _weaponSettings = _weaponSetup.weaponSettings;
            _isMoving = false;
            _meshRenderer.enabled = false;
            _thisTransform = transform;
            _moveForce = _thisTransform.forward;
            _parentRelativeLocalPos = _thisTransform.localPosition;
            _initialLocalAngles = _thisTransform.localEulerAngles;
            _damageInfo = new DamageInfo(_weaponSettings.weaponDamage);

            ScreenEdges screenWorldEdges = SIGameMasterBehaviour.Instance.ScreenAreaCalculator.CalculatedScreenEdges;
            _topWorldLimit = screenWorldEdges.topScreenEdge + _weaponSettings.movementLimitOffset;
            _bottomWorldLimit = screenWorldEdges.bottomScreenEdge - _weaponSettings.movementLimitOffset;
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
            SIGameplayEvents.OnWaveEnd += StopAndResetProjectile;
        }
        
        void RemoveEvents()
        {
            SIEventsHandler.OnUpdate -= CheckIsProjectileOnScreen;
            SIGameplayEvents.OnWaveEnd -= StopAndResetProjectile;
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
            _rigidbody.AddForce(_moveForce * _weaponSettings.launchForceMultiplier, ForceMode.Impulse);
        }

        public DamageInfo GetWeaponDamageInfo(MonoBehaviour objectToDamage)
        {
            //todo: add IDamage interaface with this method
            _damageInfo.ObjectToDamage = objectToDamage;
            return _damageInfo;
        }

        void StopAndResetProjectile()
        {
            TryToEnableParticles(false);
            _isMoving = false;
            _meshRenderer.enabled = false;
            _weaponCollider.enabled = false;

            _thisTransform.parent = _parentTransform;
            _thisTransform.localPosition = _parentRelativeLocalPos;
            _thisTransform.localRotation = Quaternion.Euler(_initialLocalAngles);
;
            _rigidbody.velocity = SIHelpers.VectorZero;
            _rigidbody.angularVelocity = SIHelpers.VectorZero;
        }

        void CheckIsProjectileOnScreen()
        {
            if (_thisTransform == null)
                return;
            
            bool isInVerticalSpace =
                SIScreenUtils.IsInVerticalWorldScreenLimit(_thisTransform.position, _bottomWorldLimit, _topWorldLimit);
            if (isInVerticalSpace)
                return;
            StopAndResetProjectile();
        }

        public void HandleProjectileHit()
        {
            StopAndResetProjectile();
        }

        void TryToEnableParticles(bool canEnableParticles)
        {
            if (_particles == null)
                return;
            
            _particles.gameObject.SetActive(canEnableParticles);

            if (canEnableParticles)
                _particles.Play();
            else
                _particles.Stop();
        }
    }
}