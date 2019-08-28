using System.Collections;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIAsteroidsMaster : MonoBehaviour
    {
        [Range(0f, 2.5f)] public float minAsteroidMoveDelay;
        [Range(2.5f, 5f)] public float maxAsteroidMoveDelay;

        [SerializeField] private SIAsteroidSpawner _asteroidsSpawner;

        protected void OnEnable()
        {
            AssignEvents();
        }

        protected void OnDisable()
        {
            RemoveEvents();
        }

        private void AssignEvents()
        {
            SIEventsHandler.OnGameStarted += InvokeAsteroidsMovement;
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnGameStarted -= InvokeAsteroidsMovement;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void InvokeAsteroidsMovement()
        {
            if (_asteroidsSpawner == null)
                return;
            
            StartCoroutine(AsteroidsMovementRoutine());
        }

        private IEnumerator AsteroidsMovementRoutine()
        {
            SIAsteroidBehaviour[] asteroids = _asteroidsSpawner.SpawnedAsteroids;
            int asteroidsCount = asteroids.Length;

            while (true)
            {
                for (int i = 0; i < asteroidsCount; i++)
                {
                    SIAsteroidBehaviour asteroid = asteroids[i];
                    asteroid.MoveObject();
                    yield return SIWaitUtils.WaitForCachedSeconds(Random.Range(minAsteroidMoveDelay,
                        maxAsteroidMoveDelay));
                }

                yield return SIWaitUtils.WaitForCachedSeconds(SIConstants.ASTEROIDS_RESPAWN_DELAY);
            }
        }
    }
}