using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    [ExecuteInEditMode]
    public class SIAsteroidSpawner : SISpawner, IRespawnable
    {
        [SerializeField] private bool test;

        [SerializeField] private float _minTimeOffset;
        [SerializeField] private float _maxTimeOffset;
        
        [SerializeField] private Transform _asteroidsTemplateParent;
        [SerializeField] private List<GameObject> _asteroidsPrefabs;
        [SerializeField] private List<SIAsteroidBehaviour> _spawnedAsteroids;

        [SerializeField] int _asteroidPrefabsCount;
        private Camera _mainCamera;

        private void Start()
        {
            Initialize();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            AssignEvents();
//            SIEventsHandler.OnWaveEnd += Spawn;
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

//            SIEventsHandler.OnWaveEnd -= Spawn;
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

        private void Initialize()
        {
            _asteroidsTemplateParent = transform;
            _asteroidPrefabsCount = _asteroidsPrefabs.Count;
        }

        private Vector3 GetOutOfScreenPosition(int parentIndex)
        {
            _mainCamera = SIGameMasterBehaviour.Instance.MainCamera;

            float xPosition;
            float yPosition;
            float zPosition = Random.Range(SIConstants.MIN_ASTEROID_Z, SIConstants.MAX_ASTEROID_Z);

            Vector3 viewportToWorldVector;
            Vector3 viewportPosition;
            
            switch (parentIndex)
            {
                case 0:
                    xPosition = SIHelpers.VIEWPORT_SPAWN_MIN;
                    yPosition = Random.Range(0f, 1f);
                    break;
                case 1:
                    xPosition = SIHelpers.VIEWPORT_SPAWN_MAX;
                    yPosition = Random.Range(0f, 1f);
                    break;
                case 2:
                    xPosition = Random.Range(0f, 1f);
                    yPosition = SIHelpers.VIEWPORT_SPAWN_MIN;
                    break;
                default:
                    xPosition = Random.Range(0f, 1f);
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

            for (int i = 0; i < SIConstants.MAX_SPAWNED_ASTEROIDS; i++)
            {
                int spawnedPrefabIndex = Random.Range(0, _asteroidPrefabsCount);
                int spawnedParentIndex = Random.Range(0, SIConstants.SCREEN_EDGES);    // skipping forward & backward colliders
                GameObject asteroidObject = Instantiate(_asteroidsPrefabs[spawnedPrefabIndex], _asteroidsTemplateParent);
                Transform asteroidTransform = asteroidObject.transform;
                asteroidTransform.position = GetOutOfScreenPosition(spawnedParentIndex);
                asteroidObject.SetActive(true);
                SIAsteroidBehaviour asteroid = asteroidObject.GetComponent<SIAsteroidBehaviour>();
                _spawnedAsteroids.Add(asteroid);
            }
        }

        private void StartAsteroidsMovements()
        {
            StartCoroutine(SpawnedObjectsRoutine());
        }

        private void ResetAsteroids()
        {
            for (int i = 0; i < SIConstants.MAX_SPAWNED_ASTEROIDS; i++)
            {
                int spawnedPrefabIndex = Random.Range(0, _asteroidPrefabsCount);
                int spawnedParentIndex = Random.Range(0, SIConstants.SCREEN_EDGES);   
                GameObject asteroidObject = _asteroidsPrefabs[spawnedPrefabIndex];
                Transform asteroidTransform = asteroidObject.transform;
                asteroidTransform.position = GetOutOfScreenPosition(spawnedParentIndex);
            }
        }

        private IEnumerator SpawnedObjectsRoutine()
        {
            int asteroidIndex;
            float timeToNextShoot = 0.0f;
            
            while (true)
            {
                for (asteroidIndex = 0; asteroidIndex < SIConstants.MAX_SPAWNED_ASTEROIDS; asteroidIndex++)
                {
                    _spawnedAsteroids[asteroidIndex].MoveObj();
                    yield return SIHelpers.CustomDelayRoutine(Random.Range(_minTimeOffset, _maxTimeOffset));
                }

                yield return SIHelpers.CustomDelayRoutine(SIConstants.ASTEROIDS_RESPAWN_DELAY/* , ResetAsteroids*/);
            }
        }

        public void Respawn()
        {
        }
    }
}