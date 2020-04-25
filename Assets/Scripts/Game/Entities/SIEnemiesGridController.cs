using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    [System.Serializable]
    public class SIShootBehaviourSetup
    {
        public int enemyIndex;
        public int enemyRow;
        public List<SIShootBehaviour> neighbours;
    }

    public class SIEnemiesGridController : MonoBehaviour
    {
        [SerializeField] GridControllerSetup _gridSetup;
        [SerializeField] SIEnemyBehaviour[] _availableEnemies;

        int _maxEnemies;
        int _livingEnemies;
        int _gridSpeedTiers;
        int _minEnemiesToUpdateGridSpeed;
        GridControllerSettings _gridSettings;

        void Awake()
        {
            PreInitialise();
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
            for (int i = 0; i < _maxEnemies; i++)
            {
                currentShootBehaviour = _availableEnemies[i];
                currentSetup = new SIShootBehaviourSetup
                {
                    enemyIndex = i,
                    enemyRow = i / _gridSettings.maxEnemiesInGridRow,
                    neighbours = GetNeighbours(i)
                };
                currentShootBehaviour.UpdateShootBehaviourSetup(currentSetup);
            }
        }

        List<SIShootBehaviour> GetNeighbours(int enemyIndex)
        {
            int maxInRow = _gridSettings.maxEnemiesInGridRow;
            int[] neightboursIndexes = new int[4];
            List<SIShootBehaviour> neightbours = new List<SIShootBehaviour>();
            
            neightboursIndexes[0] = enemyIndex - maxInRow;
            neightboursIndexes[1] = enemyIndex + maxInRow;
            neightboursIndexes[2] = enemyIndex - 1;
            neightboursIndexes[3] = enemyIndex + 1;
            
            for (int j = 0; j < 4; j++)
            {
                int nbIndex = neightboursIndexes[j];
                if (!IsPotentialNeighbourInArrayRange(nbIndex))
                    continue;
                neightbours.Add(_availableEnemies[nbIndex].ShootBehaviour);
            }

            return neightbours;
        }

        bool IsPotentialNeighbourInArrayRange(int neightbourIndex)
        {
            return neightbourIndex >= 0 && neightbourIndex < _maxEnemies;
        }
        
        void LoadSetup()
        {
            //todo: temporary
            _maxEnemies = 15;
            _livingEnemies = 15;
            _gridSettings = _gridSetup.gridControllerSettings;
            _gridSpeedTiers = _gridSettings.enemiesLeftToUpdateGridMovementTier.Length;
            _minEnemiesToUpdateGridSpeed = _gridSettings.enemiesLeftToUpdateGridMovementTier[0];
        }

        void OnEnable()
        {
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        void AssignEvents()
        {
            SIEventsHandler.OnGameStarted += HandleOnGameStarted;
            SIEventsHandler.OnEnemyDeath += HandleOnEnemyDeath;
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnGameStarted -= HandleOnGameStarted;
            SIEventsHandler.OnEnemyDeath -= HandleOnEnemyDeath;
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

            StartCoroutine(FinalizeAndRestartWaveRoutine());
        }

        void HandleOnGameStarted()
        {
            MoveEnemiesGrid();
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

        IEnumerator FinalizeAndRestartWaveRoutine()
        {
            SIEnemyGridEvents.BroadcastOnGridReset();
            yield return StartCoroutine(SIWaitUtils.WaitAndInvoke(_gridSettings.endWaveCooldown,
                SIEventsHandler.BroadcastOnWaveEnd));
            SetLivingEnemiesCount();
            yield return StartCoroutine(SIWaitUtils.WaitAndInvoke(_gridSettings.newWaveCooldown, MoveEnemiesGrid));
        }

        void SetLivingEnemiesCount()
        {
            //todo: temporary
            _livingEnemies = _maxEnemies;
        }

        void MoveEnemiesGrid()
        {
            SIEnemyGridEvents.BroadcastOnGridStarted();
        }
    }
}