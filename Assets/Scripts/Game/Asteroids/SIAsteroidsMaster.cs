using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIAsteroidsMaster : MonoBehaviour
    {
        [Range(0f, 2.5f)] public float minAsteroidMoveDelay;
        [Range(2.5f, 5f)] public float maxAsteroidMoveDelay;

        [SerializeField] SIAsteroidSpawner _asteroidsSpawner;

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

        void OnDestroy()
        {
            StopAllCoroutines();
        }

        void TryToInvokeAsteroidsMovement()
        {
            if (_asteroidsSpawner == null)
                return;

            StartCoroutine(AsteroidsMovementRoutine());
        }

        IEnumerator AsteroidsMovementRoutine()
        {
            SIAsteroidBehaviour[] asteroids = _asteroidsSpawner.SpawnedAsteroids;
            int asteroidsCount = asteroids.Length;

            while (true)
            {
                for (int i = 0; i < asteroidsCount; i++)
                {
                    SIAsteroidBehaviour asteroid = asteroids[i];
                    asteroid.MoveObject();
                    yield return WaitUtils.WaitForCachedSeconds(Random.Range(minAsteroidMoveDelay,
                        maxAsteroidMoveDelay));
                }

                yield return WaitUtils.WaitForCachedSeconds(SIConstants.ASTEROIDS_RESPAWN_DELAY);
            }
        }
    }
}