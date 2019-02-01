﻿using UnityEngine;

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
        [SerializeField] private BoxCollider2D _projectileCollider;

        private Camera _mainCamera;
        private Vector2 _moveForce;
        private Vector2 _parentResetPosition;

        public bool IsMoving { get => _isMoving; set => _isMoving = value; }

        private void Awake()
        {
            SetInitialReferences();
        }

        private void OnEnable()
        {
            ResetProjectile();
            SIEventsHandler.OnObjectMovement += CheckIsProjectileOnScreen;
            SIEventsHandler.OnEnemyDeath += ResetProjectile;
            SIEventsHandler.OnPlayerHit += ResetProjectile;
        }

        private void OnDisable()
        {
            ResetProjectile();
            SIEventsHandler.OnObjectMovement -= CheckIsProjectileOnScreen;
            SIEventsHandler.OnEnemyDeath += ResetProjectile;
            SIEventsHandler.OnPlayerHit += ResetProjectile;
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
            _spriteRenderer.enabled = false;
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
            if (_rigidbody2D == null)
            {
                return;
            }

            if (_isMoving == false)
            {
                EnableParticles(true);
                _projectileCollider.enabled = true;
                _isMoving = true;
                _spriteRenderer.enabled = true;
                _cachedProjectileTransform.parent = null;
                _rigidbody2D.AddForce(_moveForce.normalized * _forceScaleFactor, ForceMode2D.Impulse);
            }
        }

        public void ResetProjectile()
        {
            if(_cachedProjectileTransform == null || _cachedParentTransform == null || _projectileCollider == null)
            {
                Debug.Log("Assign proper values first");
                return;
            }

            EnableParticles(false);
            _projectileCollider.enabled = false;
            _isMoving = false;
            _spriteRenderer.enabled = false;
            _rigidbody2D.velocity = new Vector2(0,0);
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

