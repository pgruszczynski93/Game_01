using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyGridBehaviour : SIGenericSingleton<SIEnemyGridBehaviour>
    {

        [SerializeField] private int _totalEnemies;
        [SerializeField] private int _livingEnemies;
        [SerializeField] private int _speedMultiplier;
        [SerializeField] private SIEnemyMovement[] _movingEnemies;

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

            float newMultiplier = _livingEnemies == 1 ? 6.0f : 3.0f;

            SIEventsHandler.OnEnemySpeedMultiplierChanged?.Invoke(newMultiplier);
        }

    }
}

