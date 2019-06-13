using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    [ExecuteInEditMode]
    public class SIAsteroidSpawner : SISpawner, IRespawnable
    {
        [SerializeField] private AsteroidSpawnerConfig _spawnerConfig;
        [SerializeField] private Transform _asteroidsTemplateParent;
        [SerializeField] private List<GameObject> _asteroidsPrefabs;
        [SerializeField] private List<SIAsteroidBehaviour> _spawnedAsteroids;

        private Camera _mainCamera;

        private void Start()
        {
            Initialise();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            AssignEvents();
        }

        private void AssignEvents()
        {
            SIEventsHandler.OnSpawnObject += Spawn;
            SIEventsHandler.OnGameStarted += StartAsteroidsMovements;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            RemoveEvents();
        }

        private void RemoveEvents()
        {
            SIEventsHandler.OnSpawnObject -= Spawn;
            SIEventsHandler.OnGameStarted -= StartAsteroidsMovements;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private void Initialise()
        {
            _asteroidsTemplateParent = transform;
        }

        private Vector3 GetOutOfScreenPosition(int parentIndex)
        {
            _mainCamera = SIGameMasterBehaviour.Instance.MainCamera;

            float xPosition;
            float yPosition;
            float zPosition = Random.Range(SIConstants.MIN_ASTEROID_Z, SIConstants.MAX_ASTEROID_Z);

            float randomizedCoord = Random.Range(0f, 1f);
            Vector3 viewportToWorldVector;
            Vector3 viewportPosition;

            switch (parentIndex)
            {
                case 0:
                    xPosition = SIHelpers.VIEWPORT_SPAWN_MIN;
                    yPosition = randomizedCoord;
                    break;
                case 1:
                    xPosition = SIHelpers.VIEWPORT_SPAWN_MAX;
                    yPosition = randomizedCoord;
                    break;
                case 2:
                    xPosition = randomizedCoord;
                    yPosition = SIHelpers.VIEWPORT_SPAWN_MIN;
                    break;
                default:
                    xPosition = randomizedCoord;
                    yPosition = SIHelpers.VIEWPORT_SPAWN_MAX;
                    break;
            }

            viewportPosition = new Vector3(xPosition, yPosition, zPosition);
            viewportToWorldVector = _mainCamera.ViewportToWorldPoint(viewportPosition);
            return viewportToWorldVector;
        }


        public void Spawn()
        {
            if (_asteroidsPrefabs == null || _asteroidsPrefabs.Count == 0)
            {
                Debug.LogError("No asteroid's attached.", this);
                return;
            }

            _spawnedAsteroids = new List<SIAsteroidBehaviour>();

            for (int i = 0; i < _spawnerConfig.maxAsteroidsToSpawn; i++)
            {
                int spawnedPrefabIndex = Random.Range(0, _spawnerConfig.asteroidVariantsCount);
                int spawnedParentIndex =
                    Random.Range(0, SIConstants.SCREEN_EDGES); // skipping forward & backward colliders
                
                GameObject asteroidObject =
                    Instantiate(_asteroidsPrefabs[spawnedPrefabIndex], _asteroidsTemplateParent);
                
                Transform asteroidTransform = asteroidObject.transform;
                asteroidTransform.localPosition =
                    asteroidTransform.InverseTransformPoint(GetOutOfScreenPosition(spawnedParentIndex));
                asteroidObject.SetActive(true);
                SIAsteroidBehaviour asteroid = asteroidObject.GetComponent<SIAsteroidBehaviour>();
                _spawnedAsteroids.Add(asteroid);
            }
        }
        
        public void Respawn()
        {
        }

        private void StartAsteroidsMovements()
        {
            StartCoroutine(SpawnedObjectsRoutine());
        }

        private IEnumerator SpawnedObjectsRoutine()
        {
            int asteroidIndex;

            while (true)
            {
                for (asteroidIndex = 0; asteroidIndex < _spawnerConfig.maxAsteroidsToSpawn; asteroidIndex++)
                {
                    _spawnedAsteroids[asteroidIndex].MoveObj();
                    yield return SIHelpers.CustomDelayRoutine(Random.Range(_spawnerConfig.minTimeOffset,
                        _spawnerConfig.maxTimeOffset));
                }

                yield return SIHelpers.CustomDelayRoutine(SIConstants.ASTEROIDS_RESPAWN_DELAY);
            }
        }
    }
}