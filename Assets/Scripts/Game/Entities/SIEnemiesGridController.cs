using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemiesGridController : MonoBehaviour
    {
        [SerializeField] GridControllerSetup _gridSetup;

        int _maxLivingEnemies;
        int _livingEnemies;
        GridControllerSettings _gridSettings;

        void Initialise()
        {
            //todo: temporary
            _maxLivingEnemies = 15;
            _livingEnemies = 15;
            _gridSettings = _gridSetup.gridControllerSettings;
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
            SIEventsHandler.OnWaveEnd += HandleOnWaveEnd;
            SIEventsHandler.OnEnemyDeath += HandleOnEnemyDeath;
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnGameStarted -= HandleOnGameStarted;
            SIEventsHandler.OnWaveEnd -= HandleOnWaveEnd;
            SIEventsHandler.OnEnemyDeath -= HandleOnEnemyDeath;
        }

        void HandleOnEnemyDeath(MonoBehaviour deadEnemy)
        {
            --_livingEnemies;
            TryToBroadcastNewMovementSpeedTier();
            TryToBroadcastWaveEnd();
        }

        void TryToBroadcastWaveEnd()
        {
            if (_livingEnemies > 0)
                return;

            SIEventsHandler.BroadcastOnWaveEnd();
//            StartCoroutine(SIWaitUtils.WaitAndInvoke(SIConstants.END_WAVE_DELAY,
//                () => { SIEventsHandler.BroadcastOnWaveEnd(); }));
        }

        void HandleOnGameStarted()
        {
            MoveEnemiesGrid();
        }

        void TryToBroadcastNewMovementSpeedTier()
        {
            //todo: refactor
            if (_livingEnemies > _gridSettings.enemiesLeftToUpdateGridMovementTier[0])
                return;
            
            for (int i = 0; i < _gridSettings.enemiesLeftToUpdateGridMovementTier.Length; i++)
            {
                if (_livingEnemies == _gridSettings.enemiesLeftToUpdateGridMovementTier[i])
                {
                    SIEnemyGridEvents.BroadcastOnUpdateGridMovementSpeedTier(i);
                    return;
                }
            }
        }

        void MoveEnemiesGrid()
        {
            SIEnemyGridEvents.BroadcastOnGridStarted();
        }

        void HandleOnWaveEnd()
        {
            MoveEnemiesGridWithDelay();
        }

        void MoveEnemiesGridWithDelay()
        {
            StartCoroutine(SIWaitUtils.WaitAndInvoke(_gridSettings.newWaveCooldown, MoveEnemiesGrid));
        }
    }
}