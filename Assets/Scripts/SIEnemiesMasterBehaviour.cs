using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemiesMasterBehaviour : SIGenericSingleton<SIEnemiesMasterBehaviour>
    {
        [SerializeField] private SIEnemyMovement[] _movingEnemies;

        private int _totalEnemies;
        private int _livingEnemies;
        private int _speedMultiplier;

        protected override void Awake()
        {
            base.Awake();
            SetInitialReferences();
        }

        private void OnEnable()
        {
            SIEventsHandler.OnEnemyDeath += DecreaseEnemiesCount;
            SIEventsHandler.OnEnemyDeath += UpdateCurrentSpeedMultiplier;
        }

        private void OnDisable()
        {
            SIEventsHandler.OnEnemyDeath -= DecreaseEnemiesCount;
            SIEventsHandler.OnEnemyDeath -= UpdateCurrentSpeedMultiplier;
        }

        private void SetInitialReferences()
        {
            _totalEnemies = _movingEnemies.Length;
            _livingEnemies = _totalEnemies;
        }

        private void DecreaseEnemiesCount()
        {
            --_livingEnemies;
        }

        private void UpdateCurrentSpeedMultiplier()
        {
            if (_livingEnemies > 2)
            {
                return;
            }

            float newMultiplier = _livingEnemies == 1 ? 6.0f : 4.0f;

            SIEventsHandler.OnEnemySpeedMultiplierChanged?.Invoke(newMultiplier);
        }

    }
}

