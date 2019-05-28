using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    [ExecuteInEditMode]
    public class SIAsteroidSpawner : SISpawner, IRespawnable
    {
        [SerializeField] private bool test;

        [SerializeField] private float _shotTimeMinBreak;
        [SerializeField] private float _shotTimeMaxBreak;
        
        [SerializeField] private Transform _asteroidsTemplateParent;
        [SerializeField] private Transform[] _asteroidsBoundaries;
        [SerializeField] private List<GameObject> _asteroids;
        [SerializeField] private List<SIAsteroidBehaviour> _spawnedAsteroids;

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
        }

        private Vector3 GetOutOfScreenPosition(int parentIndex)
        {
            _mainCamera = SIGameMasterBehaviour.Instance.MainCamera;

            float xPosition;
            float yPosition;
            float zPosition = 15f;

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
            if (_asteroids == null || _asteroids.Count == 0 || _asteroidsBoundaries == null ||
                _asteroidsBoundaries.Length == 0)
            {
                Debug.LogError("No asteroid's attached.", this);
                return;
            }

            int asteroidPrefabsCount = _asteroids.Count;
            _spawnedAsteroids = new List<SIAsteroidBehaviour>();

            for (int i = 0; i < SIConstants.MAX_SPAWNED_ASTEROIDS; i++)
            {
                int spawnedPrefabIndex = Random.Range(0, asteroidPrefabsCount);
                int spawnedParentIndex = Random.Range(0, SIConstants.SCREEN_EDGES);    // skipping forward & backward colliders
                GameObject asteroidObject = Instantiate(_asteroids[spawnedPrefabIndex], _asteroidsTemplateParent);
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
                int spawnedParentIndex = Random.Range(0, SIConstants.SCREEN_EDGES);   
                GameObject asteroidObject = _asteroids[i];
                Transform asteroidTransform = asteroidObject.transform;
                asteroidTransform.localPosition = GetOutOfScreenPosition(spawnedParentIndex);
            }
        }

        private IEnumerator SpawnedObjectsRoutine()
        {
            int asteroidIndex;
            float timeToNextShoot = 0.0f;
            
            while (true)
            {
                asteroidIndex = 0;
                for (asteroidIndex = 0; asteroidIndex < SIConstants.MAX_SPAWNED_ASTEROIDS; asteroidIndex++)
                {
                    _spawnedAsteroids[asteroidIndex].MoveObj();
                    yield return SIHelpers.CustomDelayRoutine(0.55f);
                }

                yield return SIHelpers.CustomDelayRoutine(1.25f/*, () => ResetAsteroids()*/);
            }
        }

        public void Respawn()
        {
        }
    }
}