using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIAsteroidSpawner : SISpawner, IRespawnable
    {
        [SerializeField] private float _shotTimeMinBreak;
        [SerializeField] private float _shotTimeMaxBreak;
        
        [SerializeField] private Transform _asteroidsTemplateParent;
        [SerializeField] private Vector3[] _asteroidsBoundariesPositions;
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

            SetBoundariesPositions();
        }

        private void SetBoundariesPositions()
        {
            _asteroidsBoundariesPositions = new Vector3[SIConstants.ASTEROID_BOUNDARIES];
            for (int i = 0; i < SIConstants.ASTEROID_BOUNDARIES; i++)
            {
                _asteroidsBoundariesPositions[i] = _asteroidsBoundaries[i].position;
            }
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
                int spawnedParentIndex = Random.Range(0, SIConstants.ASTEROID_BOUNDARIES - 2);    // skipping forward & backward colliders
                GameObject asteroidObject = Instantiate(_asteroids[spawnedPrefabIndex], _asteroidsBoundaries[spawnedParentIndex]);
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
                asteroidIndex = Random.Range(0, SIConstants.ASTEROID_BOUNDARIES);
                timeToNextShoot = Random.Range(_shotTimeMinBreak, _shotTimeMaxBreak);
                _spawnedAsteroids[asteroidIndex].MoveObj();
                yield return SIHelpers.GetWFSCachedValue(timeToNextShoot);
            }
        }

        private bool IsPointInBoundaries(Vector3 pointCoords)
        {
            return pointCoords.x >= _asteroidsBoundariesPositions[0].x &&
                   pointCoords.x <= _asteroidsBoundariesPositions[1].x &&
                   pointCoords.y >= _asteroidsBoundariesPositions[2].y &&
                   pointCoords.y <= _asteroidsBoundariesPositions[3].y &&
                   pointCoords.z >= _asteroidsBoundariesPositions[4].z &&
                   pointCoords.z <= _asteroidsBoundariesPositions[5].z;
        }

        public void Respawn()
        {
        }
    }
}