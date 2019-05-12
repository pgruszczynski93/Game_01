using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIAsteroidSpawner : SISpawner, IRespawnable
    {
        [SerializeField] private List<GameObject> _asteroids;
        [SerializeField] private List<SIAsteroidBehaviour> _spawnedAsteroids;
        [SerializeField] private Transform _asteroidsParent;

        private void Start()
        {
            Initialize();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SIEventsHandler.OnSpawnObject += Spawn;
//            SIEventsHandler.OnWaveEnd += Spawn;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SIEventsHandler.OnSpawnObject -= Spawn;
//            SIEventsHandler.OnWaveEnd -= Spawn;
        }

        private void Initialize()
        {
            _asteroidsParent = transform;
        }

        public void Spawn()
        {
            if (_asteroids == null || _asteroids.Count == 0)
            {
                Debug.LogError("No asteroid's attached.", this);
                return;
            }
            
            Debug.Log("TTTTTTTTTTTTTTTTTTTTTT");

            int asteroidPrefabsCount = _asteroids.Count;
            _spawnedAsteroids = new List<SIAsteroidBehaviour>();
            
            for (int i = 0; i < SIConstants.MAX_SPAWNED_ASTEROIDS; i++)
            {
                Debug.Log("XXXXXXXXXXXXXXX " + i);

                int spawnedIndex = Random.Range(0, asteroidPrefabsCount);
                GameObject asteroidObject = Instantiate(_asteroids[spawnedIndex], _asteroidsParent);
                SIAsteroidBehaviour asteroid = asteroidObject.GetComponent<SIAsteroidBehaviour>();
                _spawnedAsteroids.Add(asteroid);
            }            
        }

        public void Respawn()
        {
            
        }
    }
}