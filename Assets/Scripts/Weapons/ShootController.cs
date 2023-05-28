using PG.Game.Weapons.Projectile;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PG.Game.Weapons {
    public abstract class ShootController : MonoBehaviour {
        [SerializeField] protected ProjectileSlotsParentController[] projectileSlotsParents;

        protected bool _isShootingEnabled;
        protected int _minAvailableProjectiles;
        protected int _maxAvailableProjectiles;
        protected int _availableProjectilesCount;
        public bool IsShootingEnabled => _isShootingEnabled;

        protected void Start() => Initialise();

        protected virtual void Initialise() {
            _isShootingEnabled = true;
            SetAvailableProjectilesRangeCount();
        }

        protected void OnEnable() => SubscribeEvents();
        protected void OnDisable() => UnsubscribeEvents();

        protected virtual void SubscribeEvents() { }
        protected virtual void UnsubscribeEvents() { }

        public Transform[] GetProjectileSlotsParent() {
            return projectileSlotsParents[_availableProjectilesCount - 1].ProjectilesSlotsTransforms;
        }

        protected void EnableShooting(bool isEnabled) {
            _isShootingEnabled = isEnabled;
        }

        protected void UpdateAvailableProjectilesCount() {
            ++_availableProjectilesCount;
            _availableProjectilesCount = Mathf.Clamp(_availableProjectilesCount, _minAvailableProjectiles,
                _maxAvailableProjectiles);
        }

        protected void ResetAvailableProjectilesCount() {
            _availableProjectilesCount = 0;
        }

        [Button]
        protected void SetAvailableProjectilesRangeCount() {
            _minAvailableProjectiles = 1;
            _maxAvailableProjectiles = projectileSlotsParents.Length;
            _availableProjectilesCount = _minAvailableProjectiles;
        }
    }
}