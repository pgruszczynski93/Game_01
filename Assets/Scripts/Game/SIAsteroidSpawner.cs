using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    [ExecuteInEditMode]
    public class SIAsteroidSpawner : SISpawner, IConfigurableObject, IRespawnable
    {
        [SerializeField] private AsteroidSpawnerSettings _spawnerSettings;
        [SerializeField] private Transform _asteroidsTemplateParent;
        [SerializeField] private SIAsteroidBehaviour[] _spawnedAsteroids;

        private float _cameraZ;
        private Camera _mainCamera;

        private void Start()
        {
            Initialise();
        }
        
        public void Configure(ScriptableSettingsMaster settings)
        {
            _spawnerSettings = settings.asteroidSpawnerConfigurator.asteroidSpawnerSettings;
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

            float xPosition;
            float yPosition;
            float zPosition = Random.Range(SIConstants.MIN_ASTEROID_Z, SIConstants.MAX_ASTEROID_Z) - _cameraZ;

            float randomizedCoord = Random.Range(0f, 1f);

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
            

            Vector3 viewportPosition = new Vector3(xPosition, yPosition, zPosition);
            Vector3 viewportToWorldVector = _mainCamera.ViewportToWorldPoint(viewportPosition);
            return viewportToWorldVector;
        }


        public void Spawn()
        {
            if (_spawnerSettings.asteroidVariants == null || _spawnerSettings.asteroidVariants.Length == 0)
            {
                Debug.LogError("No asteroid's attached.", this);
                return;
            }

            _mainCamera = SIGameMasterBehaviour.Instance.MainCamera;
            _cameraZ = _mainCamera.transform.position.z;
            _spawnedAsteroids = new SIAsteroidBehaviour[_spawnerSettings.maxAsteroidsToSpawn];

            for (int i = 0; i < _spawnerSettings.maxAsteroidsToSpawn; i++)
            {
                int spawnedPrefabIndex = Random.Range(0, _spawnerSettings.asteroidVariantsCount);
                int spawnedParentIndex = Random.Range(0, SIConstants.SCREEN_EDGES);
                
                GameObject asteroidObject =
                    Instantiate(_spawnerSettings.asteroidVariants[spawnedPrefabIndex], _asteroidsTemplateParent);
                
                Transform asteroidTransform = asteroidObject.transform;
                asteroidTransform.localPosition =
                    asteroidTransform.InverseTransformPoint(GetOutOfScreenPosition(spawnedParentIndex));
                asteroidObject.SetActive(true);
                SIAsteroidBehaviour asteroid = asteroidObject.GetComponent<SIAsteroidBehaviour>();
                _spawnedAsteroids[i] = asteroid;
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
                for (asteroidIndex = 0; asteroidIndex < _spawnerSettings.maxAsteroidsToSpawn; asteroidIndex++)
                {
                    _spawnedAsteroids[asteroidIndex].MoveObject();
                    yield return SIHelpers.CustomDelayRoutine(Random.Range(_spawnerSettings.minTimeOffset,
                        _spawnerSettings.maxTimeOffset));
                }

                yield return SIHelpers.CustomDelayRoutine(SIConstants.ASTEROIDS_RESPAWN_DELAY);
            }
        }
    }
}