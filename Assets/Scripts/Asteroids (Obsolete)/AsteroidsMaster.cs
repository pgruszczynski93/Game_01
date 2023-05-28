using System.Threading;
using Cysharp.Threading.Tasks;
using PG.Game.EventSystem;
using PG.Game.Helpers;
using PG.Game.Systems;
using UnityEngine;

namespace PG.Game.Asteroids {
    public class AsteroidsMaster : MonoBehaviour {
        [Range(0f, 2.5f)] public float minAsteroidMoveDelay;
        [Range(2.5f, 5f)] public float maxAsteroidMoveDelay;

        [SerializeField] AsteroidSpawner _asteroidsSpawner;

        CancellationTokenSource _asteroidsCancellation;

        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();

        void SubscribeEvents() {
            GeneralEvents.OnGameStateChanged += HandleOnGameStateChanged;
        }

        void UnsubscribeEvents() {
            GeneralEvents.OnGameStateChanged += HandleOnGameStateChanged;
        }

        void HandleOnGameStateChanged(GameStates gameState) {
            if (gameState != GameStates.GameStarted)
                return;

            TryToInvokeAsteroidsMovement();
        }

        void TryToInvokeAsteroidsMovement() {
            if (_asteroidsSpawner == null)
                return;

            RefreshCancellation();
            AsteroidsMovementTask().Forget();
        }

        async UniTaskVoid AsteroidsMovementTask() {
            AsteroidBehaviour[] asteroids = _asteroidsSpawner.SpawnedAsteroids;
            int asteroidsCount = asteroids.Length;

            while (true) {
                for (int i = 0; i < asteroidsCount; i++) {
                    AsteroidBehaviour asteroid = asteroids[i];
                    asteroid.MoveObject();
                    await WaitUtils.WaitSecondsTask(Random.Range(minAsteroidMoveDelay, maxAsteroidMoveDelay), _asteroidsCancellation.Token);
                }

                object SIConstants;
                await WaitUtils.WaitSecondsTask(Consts.ASTEROIDS_RESPAWN_DELAY, _asteroidsCancellation.Token);
            }
        }

        void RefreshCancellation() {
            _asteroidsCancellation?.Cancel();
            _asteroidsCancellation?.Dispose();
            _asteroidsCancellation = new CancellationTokenSource();
        }
    }
}