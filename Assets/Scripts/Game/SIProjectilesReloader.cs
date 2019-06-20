using UnityEngine;

namespace SpaceInvaders
{
    public class SIProjectilesReloader : MonoBehaviour
    {
        [SerializeField] private int _projectileIndex;
        [SerializeField] private int _availableProjectilesCount;
        [SerializeField] private float _nextReloadTime;
        [SerializeField] private float _reloadTime;
        [SerializeField] private SIProjectileBehaviour _currentProjectile;
        [SerializeField] private GameObject[] _availableProjectiles;

        private void Awake()
        {
            SetInitialReferences();
        }
        
        private void SetInitialReferences()
        {
            if (_availableProjectiles == null ||
                _reloadTime == 0)
            {
                SIHelpers.SISimpleLogger(this, "Properties aren't assigned.", SimpleLoggerTypes.Error);
                return;
            }

            _availableProjectilesCount = _availableProjectiles.Length;
        }

        public void ShootAndReloadProjectile()
        {
            float timeFromStart = Time.time;
            if (timeFromStart > _nextReloadTime)
            {
                _nextReloadTime = timeFromStart + _reloadTime;

                if (CanShootCurrentProjectile() == false)
                {
                    SIHelpers.SISimpleLogger(this, "Shoot failed! - No SIProjectileBehaviour component attached", SimpleLoggerTypes.Error);
                    return;
                }

                SetNextProjectile();
            }
        }

        private bool CanShootCurrentProjectile()
        {
            _currentProjectile = _availableProjectiles[_projectileIndex].GetComponent<SIProjectileBehaviour>();
            if (_currentProjectile == null)
            {
                return false;
            }

            _currentProjectile.MoveProjectile();
            return true;
        }

        private void SetNextProjectile()
        {
            ++_projectileIndex;
            if (_projectileIndex > _availableProjectilesCount - 1)
            {
                _projectileIndex = 0;
            }
        }
    }
}