using Sirenix.OdinInspector;
using SpaceInvaders.ObjectsPool;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace SpaceInvaders
{
    public class SIProjectileEntity : MonoBehaviour, IPoolable
    {
        [SerializeField] SIProjectileSetup _projectileSetup;
        [SerializeField] SIProjectileSettings _projectileSettings;
        [SerializeField] Rigidbody _rigidbody;
        [SerializeField] Transform _parentTransform;
        [SerializeField] ParticleSystem _particles;
        [SerializeField] BoxCollider _weaponCollider;
        [SerializeField] GameObject _weaponGraphicsObj;
        [SerializeField] Transform _graphicsObjParent;

        bool _initialised;
        bool _isMoving;

        float _topWorldLimit;
        float _bottomWorldLimit;
        Vector3 _moveDirection;
        Vector3 _parentRelativeLocalPos;
        Transform _thisTransform;
        DamageInfo _damageInfo;
        ProjectileOwnerTag _ownerTag;

        void OnEnable() {
            Initialise();
            SubscribeEvents();
        }

        void OnDisable() {
            UnsubscribeEvents();
        }

        //TODO: Draw projectile gizmo correctly or make a tool for bullets
        // #if UNITY_EDITOR
        // void OnDrawGizmos() {
        //     Gizmos.color = Color.green;
        //     var myTransform = transform;
        //     var myLocalScale = myTransform.localScale;
        //     var colliderSize = _weaponCollider.size;
        //     var colliderCenter = _weaponCollider.center;
        //     //Note: Y and Z axis are swapped intentionally to avoid redundant rotation matrix.
        //     //This gizmo is only to setup projectile fbx correctly and values are experimental.
        //     Gizmos.DrawWireCube(
        //         myTransform.position + new Vector3(colliderCenter.x, 0.35f, colliderCenter.y), 
        //         new Vector3(colliderSize.x * myLocalScale.x, 
        //             colliderSize.z * myLocalScale.z, 
        //             colliderSize.y * myLocalScale.y));
        // }
        // #endif

        [Button]
        void SetupProjectileFromPrefabMode() {
            SetupProjectile(_projectileSetup);
        }
            
        public void SetupProjectile(SIProjectileSetup setup) {
            SIProjectileSettings projectileSettings = setup.projectileSettings;
            _weaponGraphicsObj = Instantiate(projectileSettings.projectileObject, _graphicsObjParent);
            _ownerTag = projectileSettings.ownerTag;
            Transform graphicsObjTransform = _weaponGraphicsObj.transform;
            // Note: Parameters of object in template (transform relative to parent)
            graphicsObjTransform.localPosition = projectileSettings.parentRelativePos;
            graphicsObjTransform.localRotation = Quaternion.Euler(projectileSettings.parentRelativeRotation);
            graphicsObjTransform.localScale = projectileSettings.scaleValues;
        }

        public void SetParent(Transform parent) {
            _parentTransform = parent;
        }
        
        void Initialise()
        {
            if(_initialised)
                return;

            _initialised = true;
            _projectileSettings = _projectileSetup.projectileSettings;
            _isMoving = false;
            _weaponGraphicsObj.SetActive(false);
            _thisTransform = transform;
            _parentRelativeLocalPos = _thisTransform.localPosition;
            _damageInfo = new DamageInfo(_projectileSettings.projectileDamage);

            ScreenEdges screenWorldEdges = SIGameMasterBehaviour.Instance.ScreenAreaCalculator.CalculatedScreenEdges;
            _topWorldLimit = screenWorldEdges.topScreenEdge + _projectileSettings.movementLimitOffset;
            _bottomWorldLimit = screenWorldEdges.bottomScreenEdge - _projectileSettings.movementLimitOffset;
        }

        void SubscribeEvents()
        {
            SIEventsHandler.OnUpdate += CheckIsProjectileOnScreen;
            SIGameplayEvents.OnDamage += HandleOnDamage;
        }

        void UnsubscribeEvents()
        {
            SIEventsHandler.OnUpdate -= CheckIsProjectileOnScreen;
            SIGameplayEvents.OnDamage -= HandleOnDamage;
        }
        
        public void TryToLaunchWeapon()
        {
            if (_isMoving)
                return;

            _isMoving = true;
            _weaponGraphicsObj.SetActive(true);
            _weaponCollider.enabled = true;
            _thisTransform.parent = null;
            
            TryToEnableParticles(true);
            _rigidbody.AddForce(_moveDirection * _projectileSettings.launchForceMultiplier, ForceMode.Impulse);
        }

        public DamageInfo GetWeaponDamageInfo(MonoBehaviour objectToDamage)
        {
            _damageInfo.ObjectToDamage = objectToDamage;
            return _damageInfo;
        }

        void StopAndResetProjectile()
        {
            TryToEnableParticles(false);
            _isMoving = false;
            _weaponGraphicsObj.SetActive(false);
            _weaponCollider.enabled = false;

            _thisTransform.parent = _parentTransform;
            _thisTransform.localPosition = _parentRelativeLocalPos;
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
        
        void HandleOnDamage(DamageInfo damageInfo) {
            //Note:
            //This method is added to handle damage given by laser beam.
            //Usually explosion pool manages spawning explosion particles when 2 colliders colliding.
            if (damageInfo.ObjectToDamage != this)
                return;
            
            SIGameplayEvents.BroadcastOnExplosiveObjectHit(transform.position);
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

        public void UseObjectFromPool() {
            TryToLaunchWeapon();
        }

        public void SetSpawnPosition(Vector3 spawnPos) {
            _thisTransform.position = spawnPos;
        }

        public void SetSpawnRotation(Vector3 spawnRot) {
            _thisTransform.rotation = Quaternion.LookRotation(spawnRot, Vector3.forward);
            _moveDirection = spawnRot;
        }
    }
}