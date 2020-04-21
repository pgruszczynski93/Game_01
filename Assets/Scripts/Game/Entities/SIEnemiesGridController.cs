using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemiesGridController : MonoBehaviour
    {
        [SerializeField] GridControllerSetup _gridSetup;

        int _maxLivingEnemies;
        int _livingEnemies;
        int _gridSpeedTiers;
        int _minEnemiesToUpdateGridSpeed;
        GridControllerSettings _gridSettings;

        void Initialise()
        {
            //todo: temporary
            _maxLivingEnemies = 15;
            _livingEnemies = 15;
            _gridSettings = _gridSetup.gridControllerSettings;
            _gridSpeedTiers = _gridSettings.enemiesLeftToUpdateGridMovementTier.Length;
            _minEnemiesToUpdateGridSpeed = _gridSettings.enemiesLeftToUpdateGridMovementTier[0];
        }

        void Start()
        {
            Initialise();
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
            _livingEnemies = _maxLivingEnemies;
        }

        void MoveEnemiesGrid()
        {
            SIEnemyGridEvents.BroadcastOnGridStarted();
        }
    }
}