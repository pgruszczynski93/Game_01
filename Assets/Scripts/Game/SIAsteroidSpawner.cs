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

        private void Start()
        {
            Initialize();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SIEventsHandler.OnSpawnObject += Spawn;
            SIEventsHandler.OnGameStarted += StartAsteroidsMovements;
//            SIEventsHandler.OnWaveEnd += Spawn;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SIEventsHandler.OnSpawnObject -= Spawn;
            SIEventsHandler.OnGameStarted -= StartAsteroidsMovements;

//            SIEventsHandler.OnWaveEnd -= Spawn;
        }

        private void Initialize()
        {
            _asteroidsTemplateParent = transform;
        }

        private Vector3 GetOutOfScreenPosition(int parentIndex)
        {
            Camera mainCamera = SIGameMasterBehaviour.Instance.MainCamera;

            float xPosition;
            float yPosition;
            float zPosition = 12f;

            float maxPosition = SIConstants.VIEWPORT_SPAWN_MAX;
            float minPosition = SIConstants.VIEWPORT_SPAWN_MIN;

            Vector3 viewportToWorldVector;
            
            switch (parentIndex)
            {
                case 0:
                    xPosition = -minPosition;
                    yPosition = Random.Range(0f, 1f);
                    break;
                case 1:
                    xPosition = maxPosition;
                    yPosition = Random.Range(0f, 1f);
                    break;
                case 2:
                    xPosition = Random.Range(0f, 1f);
                    yPosition = -minPosition;
                    break;
                default:
                    xPosition = Random.Range(0f, 1f);
                    yPosition = maxPosition;
                    break;
            }
            
            
            viewportToWorldVector = mainCamera.ViewportToWorldPoint(new Vector3(xPosition, yPosition, zPosition));
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
            Transform playerTarget = SIGameMasterBehaviour.Instance.Player.transform;

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
                    yield return SIHelpers.CustomDelayRoutine(1.25f);
                }

                yield return SIHelpers.CustomDelayRoutine(1.25f);
            }
        }

        public void Respawn()
        {
        }
    }
}