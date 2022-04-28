using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIAsteroidsMaster : MonoBehaviour
    {
        [Range(0f, 2.5f)] public float minAsteroidMoveDelay;
        [Range(2.5f, 5f)] public float maxAsteroidMoveDelay;

        [SerializeField] SIAsteroidSpawner _asteroidsSpawner;

        CancellationTokenSource _cancellationTokenSource;
        
        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();

        void SubscribeEvents()
        {
            SIEventsHandler.OnGameStateChanged += HandleOnGameStateChanged;
        }

        void UnsubscribeEvents()
        {
            SIEventsHandler.OnGameStateChanged += HandleOnGameStateChanged;
        }
        
        void HandleOnGameStateChanged(GameStates gameState)
        {
            if (gameState != GameStates.GameStarted)
                return;
            
            TryToInvokeAsteroidsMovement();
        }
        
        void TryToInvokeAsteroidsMovement()
        {
            if (_asteroidsSpawner == null)
                return;

            RefreshCancellationSource();
            AsteroidsMovementTask().Forget();
        }

        async UniTaskVoid AsteroidsMovementTask()
        {
            SIAsteroidBehaviour[] asteroids = _asteroidsSpawner.SpawnedAsteroids;
            int asteroidsCount = asteroids.Length;

            while (true)
            {
                for (int i = 0; i < asteroidsCount; i++)
                {
                    SIAsteroidBehaviour asteroid = asteroids[i];
                    asteroid.MoveObject();
                    await WaitForUtils.WaitSecondsTask(Random.Range(minAsteroidMoveDelay, maxAsteroidMoveDelay), _cancellationTokenSource.Token);
                }

                await WaitForUtils.WaitSecondsTask(SIConstants.ASTEROIDS_RESPAWN_DELAY, _cancellationTokenSource.Token);
            }
        }

        void RefreshCancellationSource() {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }
}