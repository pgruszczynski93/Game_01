using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemiesGridSpawner : MonoBehaviour
    {
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
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnGameStarted -= HandleOnGameStarted;
            SIEventsHandler.OnWaveEnd -= HandleOnWaveEnd;
        }

        void HandleOnGameStarted()
        {
            MoveEnemiesGrid();
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
            StartCoroutine(SIWaitUtils.WaitAndInvoke(SIConstants.NEW_WAVE_COOLDOWN, MoveEnemiesGrid));
        }
    }
}