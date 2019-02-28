using UnityEngine;

namespace SpaceInvaders
{
    public class SIProjectilesReloader : MonoBehaviour
    {
        [SerializeField] private int _projectileIndex;
        [SerializeField] private int _availableProjectilesCount;
        [SerializeField] private float _nextReloadTime;
        [SerializeField] private float _reloadTime;
        [SerializeField] private GameObject[] _availableProjectiles;

        private void Awake()
        {
            SetInitialReferences();
        }
        
        private void OnEnable()
        {
            SIEventsHandler.OnPlayerShoot += ReloadProjectile;
        }

        private void OnDisable()
        {
            SIEventsHandler.OnPlayerShoot -= ReloadProjectile;
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

        private void ReloadProjectile()
        {
            float timeFromStart = Time.time;
            if (Input.GetKeyDown(KeyCode.R) && timeFromStart > _nextReloadTime)
            {
                _nextReloadTime = timeFromStart + _reloadTime;
                Debug.Log("REload " + _availableProjectiles[_projectileIndex].name);
                GetNextProjectile();
            }
        }

        private void GetNextProjectile()
        {
            ++_projectileIndex;
            if (_projectileIndex > _availableProjectilesCount - 1)
            {
                _projectileIndex = 0;
            }
        }
    }
}