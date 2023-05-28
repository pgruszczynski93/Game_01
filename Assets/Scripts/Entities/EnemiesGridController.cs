using System.Collections.Generic;
using PG.Game.Configs;
using PG.Game.Entities.Enemies;
using PG.Game.EventSystem;
using UnityEngine;
using WaveType = PG.Game.Systems.WaveSystem.WaveType;

namespace PG.Game.Entities {
    public enum Neighbour {
        Left,
        Right,
        Front,
        Back
    }

    [System.Serializable]
    public class SIShootBehaviourSetup {
        public int enemyIndex;
        public int enemyRow;
        public int enemyColumn;
        public Dictionary<Neighbour, EnemyBehaviour> neighbours;
    }

    public class EnemiesGridController : MonoBehaviour {
        [SerializeField] GridControllerSetup _gridSetup;
        [SerializeField] EnemyBehaviour[] _availableEnemies;
        [SerializeField] GridMovementBehaviour _gridMovementBehaviour;

        int _maxEnemies;
        int _maxInRow;
        int _maxInColumn;
        int _maxNeighboursOfEnemyCount;
        int _livingEnemies;
        GridControllerSettings _gridSettings;

        void Awake() {
            PreInitialise();
        }

        void OnEnable() {
            SubscribeEvents();
        }

        void OnDisable() {
            UnsubscribeEvents();
        }

        void SubscribeEvents() {
            GameplayEvents.OnEnemyDeath += HandleOnEnemyDeath;
            GameplayEvents.OnWaveEnd += HandleOnWaveEnd;
        }

        void UnsubscribeEvents() {
            GameplayEvents.OnEnemyDeath -= HandleOnEnemyDeath;
            GameplayEvents.OnWaveEnd -= HandleOnWaveEnd;
        }

        void PreInitialise() {
            LoadSetup();
            SetupEnemies();
        }

        void HandleOnEnemyDeath(MonoBehaviour deadEnemy) {
            --_livingEnemies;
            _gridMovementBehaviour.UpdateCurrentMovementSpeed();
        }


        void HandleOnWaveEnd(WaveType waveType) {
            if (waveType == WaveType.Grid)
                TryResetGridController();
        }

        void SetupEnemies() {
            EnemyBehaviour currentShootBehaviour;
            SIShootBehaviourSetup currentSetup;
            int currentRow, currentColumn;
            for (int i = 0; i < _maxEnemies; i++) {
                currentRow = i / _gridSettings.maxEnemiesInGridRow;
                currentColumn = i / _gridSettings.maxEnemiesInGridColumn;
                currentShootBehaviour = _availableEnemies[i];
                currentSetup = new SIShootBehaviourSetup {
                    enemyIndex = i,
                    enemyRow = currentRow,
                    enemyColumn = currentColumn,
                    neighbours = GetNeighbours(i, currentRow)
                };
                currentShootBehaviour.UpdateShootBehaviourSetup(currentSetup);
            }
        }

        Dictionary<Neighbour, EnemyBehaviour> GetNeighbours(int enemyIndex, int enemyRow) {
            int leftNbIndex = enemyIndex - 1;
            int rightNbIndex = enemyIndex + 1;
            int frontNbIndex = enemyIndex + _maxInRow;
            int backNbIndex = enemyIndex - _maxInRow;

            return new Dictionary<Neighbour, EnemyBehaviour> {
                { Neighbour.Left, IsInRowHorizontalRange(leftNbIndex, enemyRow) ? _availableEnemies[leftNbIndex] : null }, {
                    Neighbour.Right,
                    IsInRowHorizontalRange(rightNbIndex, enemyRow) ? _availableEnemies[rightNbIndex] : null
                },
                { Neighbour.Front, IsInMinMaxRange(frontNbIndex) ? _availableEnemies[frontNbIndex] : null },
                { Neighbour.Back, IsInMinMaxRange(backNbIndex) ? _availableEnemies[backNbIndex] : null }
            };
        }

        bool IsInRowHorizontalRange(int currentNeighbourIndex, int enemyRow) {
            int minHorizontal = enemyRow * _maxInRow;
            int maxHorizontal = minHorizontal + _maxInRow;
            return currentNeighbourIndex >= minHorizontal && currentNeighbourIndex < maxHorizontal;
        }

        bool IsInMinMaxRange(int currentNeighbourIndex) {
            return currentNeighbourIndex >= 0 && currentNeighbourIndex < _maxEnemies;
        }

        void LoadSetup() {
            _gridSettings = _gridSetup.gridControllerSettings;
            _maxEnemies = _gridSettings.maxEnemiesInGrid;
            _livingEnemies = _maxEnemies;
            _maxNeighboursOfEnemyCount = _gridSettings.maxNeighboursOfEnemyCount;
            _maxInColumn = _gridSettings.maxEnemiesInGridColumn;
            _maxInRow = _gridSettings.maxEnemiesInGridRow;
        }


        void TryResetGridController() {
            if (_livingEnemies > 0)
                return;

            _livingEnemies = _maxEnemies;
        }
    }
}