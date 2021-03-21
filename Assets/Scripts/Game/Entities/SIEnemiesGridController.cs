using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public enum Neighbour
    {
        Left,
        Right,
        Front,
        Back,
    }

    [System.Serializable]
    public class SIShootBehaviourSetup
    {
        public int enemyIndex;
        public int enemyRow;
        public int enemyColumn;
        public Dictionary<Neighbour, SIEnemyBehaviour> neighbours;
    }

    public class SIEnemiesGridController : MonoBehaviour
    {
        [SerializeField] GridControllerSetup _gridSetup;
        [SerializeField] SIEnemyBehaviour[] _availableEnemies;

        int _maxEnemies;
        int _maxInRow;
        int _maxInColumn;
        int _maxNeighboursOfEnemyCount;
        int _livingEnemies;
        int _gridSpeedTiers;
        int _minEnemiesToUpdateGridSpeed;
        GridControllerSettings _gridSettings;

        void Awake() => PreInitialise();
        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();
        
        void SubscribeEvents()
        {
            SIEventsHandler.OnGameStateChanged += HandleOnGameStateChanged;
            SIGameplayEvents.OnEnemyDeath += HandleOnEnemyDeath;
        }

        void UnsubscribeEvents()
        {
            SIEventsHandler.OnGameStateChanged -= HandleOnGameStateChanged;
            SIGameplayEvents.OnEnemyDeath -= HandleOnEnemyDeath;
        }

        void HandleOnGameStateChanged(GameStates gameState)
        {
            if (gameState != GameStates.GameStarted)
                return;
            
            StartCoroutine(RestartGridRoutine());
        }

        void PreInitialise()
        {
            LoadSetup();
            SetupEnemies();
        }

        void SetupEnemies()
        {
            SIEnemyBehaviour currentShootBehaviour;
            SIShootBehaviourSetup currentSetup;
            int currentRow, currentColumn;
            for (int i = 0; i < _maxEnemies; i++)
            {
                currentRow = i / _gridSettings.maxEnemiesInGridRow;
                currentColumn = i / _gridSettings.maxEnemiesInGridColumn;
                currentShootBehaviour = _availableEnemies[i];
                currentSetup = new SIShootBehaviourSetup
                {
                    enemyIndex = i,
                    enemyRow = currentRow,
                    enemyColumn = currentColumn,
                    neighbours = GetNeighbours(i, currentRow),
                };
                currentShootBehaviour.UpdateShootBehaviourSetup(currentSetup);
            }
        }

        Dictionary<Neighbour, SIEnemyBehaviour> GetNeighbours(int enemyIndex, int enemyRow)
        {
            int leftNbIndex = enemyIndex - 1;
            int rightNbIndex = enemyIndex + 1;
            int frontNbIndex = enemyIndex + _maxInRow;
            int backNbIndex = enemyIndex - _maxInRow;

            return new Dictionary<Neighbour, SIEnemyBehaviour>
            {
                {Neighbour.Left, IsInRowHorizontalRange(leftNbIndex, enemyRow) ? _availableEnemies[leftNbIndex] : null},
                {
                    Neighbour.Right,
                    IsInRowHorizontalRange(rightNbIndex, enemyRow) ? _availableEnemies[rightNbIndex] : null
                },
                {Neighbour.Front, IsInMinMaxRange(frontNbIndex) ? _availableEnemies[frontNbIndex] : null},
                {Neighbour.Back, IsInMinMaxRange(backNbIndex) ? _availableEnemies[backNbIndex] : null}
            };
        }

        bool IsInRowHorizontalRange(int currentNeighbourIndex, int enemyRow)
        {
            int minHorizontal = enemyRow * _maxInRow;
            int maxHorizontal = minHorizontal + _maxInRow;
            return currentNeighbourIndex >= minHorizontal && currentNeighbourIndex < maxHorizontal;
        }

        bool IsInMinMaxRange(int currentNeighbourIndex)
        {
            return currentNeighbourIndex >= 0 && currentNeighbourIndex < _maxEnemies;
        }

        void LoadSetup()
        {
            _gridSettings = _gridSetup.gridControllerSettings;
            _maxEnemies = _gridSettings.maxEnemiesInGrid;
            _livingEnemies = _maxEnemies;
            _maxNeighboursOfEnemyCount = _gridSettings.maxNeighboursOfEnemyCount;
            _maxInColumn = _gridSettings.maxEnemiesInGridColumn;
            _maxInRow = _gridSettings.maxEnemiesInGridRow;
            _gridSpeedTiers = _gridSettings.enemiesLeftToUpdateGridMovementTier.Length;
            _minEnemiesToUpdateGridSpeed = _gridSettings.enemiesLeftToUpdateGridMovementTier[0];
        }
        

        void HandleOnEnemyDeath(MonoBehaviour deadEnemy)
        {
            --_livingEnemies;
            TryToBroadcastNewMovementSpeedTier();
            TryToFinalizeWave();
        }

        void TryToFinalizeWave()
        {
            if (_livingEnemies > 0)
                return;

            StartCoroutine(RestartGridRoutine());
        }
        

        void TryToBroadcastNewMovementSpeedTier()
        {
            if (_livingEnemies > _minEnemiesToUpdateGridSpeed)
                return;

            for (int i = 0; i < _gridSpeedTiers; i++)
            {
                if (_livingEnemies != _gridSettings.enemiesLeftToUpdateGridMovementTier[i])
                    continue;

                SIEnemyGridEvents.BroadcastOnUpdateGridMovementSpeedTier(i);
                return;
            }
        }

        IEnumerator RestartGridRoutine()
        {
            yield return StartCoroutine(WaitUtils.WaitAndInvoke(_gridSettings.endWaveCooldown,
                SIGameplayEvents.BroadcastOnWaveEnd));
            SetLivingEnemiesCount();
            yield return StartCoroutine(WaitUtils.SkipFramesAndInvoke(1,
                ReloadGridObjects));
            yield return StartCoroutine(WaitUtils.WaitAndInvoke(_gridSettings.newWaveCooldown, RestartEnemiesGrid));
        }

        void SetLivingEnemiesCount()
        {
            //todo: temporary
            _livingEnemies = _maxEnemies;
        }
        
        void ReloadGridObjects()
        {
            SIEnemyGridEvents.BroadcastOnGridObjectsReloaded();
        }

        void RestartEnemiesGrid()
        {
            SIEnemyGridEvents.BroadcastOnGridReset();
        }
    }
}