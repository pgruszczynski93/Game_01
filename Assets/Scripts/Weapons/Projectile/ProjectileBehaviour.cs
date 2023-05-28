using PG.Game.Collisions;
using PG.Game.Configs;
using PG.Game.EventSystem;
using PG.Game.Features.ObjectsPool;
using PG.Game.Helpers;
using PG.Game.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PG.Game.Weapons.Projectile {
    public class ProjectileBehaviour : MonoBehaviour, IPoolable {
        [SerializeField] SIProjectileSetup _projectileSetup;
        [SerializeField] SIProjectileSettings _projectileSettings;
        [SerializeField] Transform _parentTransform;
        [SerializeField] ParticleSystem _particles;
        [SerializeField] BoxCollider _weaponCollider;
        [SerializeField] Transform _graphicsObjParent;
        [SerializeField] ProjectileOwnerTag _ownerTag;
        [SerializeField] GameObject _projectileContent;
        [SerializeField] ProjectileMovement _projectileMovement;

        DamageInfo _damageInfo;
        GameObject _weaponGraphicsObj;
        Transform _thisTransform;

        void Start() => Initialise();

        void OnEnable() {
            Initialise();
            SubscribeEvents();
        }

        void OnDisable() => UnsubscribeEvents();

        void SubscribeEvents() {
            GameplayEvents.OnDamage += HandleOnDamage;
        }

        void UnsubscribeEvents() {
            GameplayEvents.OnDamage -= HandleOnDamage;
        }

        void Initialise() {
            _thisTransform = transform;
            _projectileSettings = _projectileSetup.projectileSettings;
            _damageInfo = new DamageInfo(_projectileSettings.projectileDamage);
            _projectileMovement.Initialise(_ownerTag, _projectileSettings.launchForceMultiplier);
        }

        void HandleOnDamage(DamageInfo damageInfo) {
            //Note: This method is added to handle damage given by laser beam.
            //Usually explosion pool manages spawning explosion particles when 2 colliders colliding.
            if (damageInfo.ObjectToDamage != this) return;

            GameplayEvents.BroadcastOnExplosiveObjectHit(transform.position);
            StopAndResetProjectile();
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

        public DamageInfo GetWeaponDamageInfo(MonoBehaviour objectToDamage) {
            _damageInfo.ObjectToDamage = objectToDamage;
            return _damageInfo;
        }

        public void HandleProjectileHit() {
            StopAndResetProjectile();
        }

        public void PerformOnPoolActions() {
            UseProjectile();
        }

        public void SetSpawnPosition(Vector3 spawnPos) {
            _projectileMovement.SetMovePosition(spawnPos);
        }

        public void SetSpawnRotation(Vector3 spawnRot) {
            _projectileMovement.SetMoveDirection(spawnRot);
        }

        public void ManageScreenVisibility() {
            if (_thisTransform && ScreenUtils.IsInVerticalWorldScreenLimit(_thisTransform.position.y))
                return;

            StopAndResetProjectile();
        }

        void UseProjectile() {
            _projectileContent.SetActive(true);
            _weaponCollider.enabled = true;

            TryToEnableParticles(true);

            _projectileMovement.MoveObject();
        }

        void StopAndResetProjectile() {
            TryToEnableParticles(false);
            _projectileContent.SetActive(false);
            _weaponCollider.enabled = false;
            _projectileMovement.StopObject();
            _projectileMovement.SetParent(_parentTransform);
        }

        void TryToEnableParticles(bool canEnableParticles) {
            if (_particles == null) return;

            _particles.gameObject.SetActive(canEnableParticles);

            if (canEnableParticles)
                _particles.Play();
            else
                _particles.Stop();
        }

        [Button]
        void SetupProjectileFromPrefabMode() {
            SetupProjectile(_projectileSetup);
        }
    }
}