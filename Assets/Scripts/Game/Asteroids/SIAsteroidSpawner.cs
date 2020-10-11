using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceInvaders
{
    public class SIAsteroidSpawner : MonoBehaviour
    {
        [SerializeField] AsteroidSpawnerSetup _spawnerSetup;
        [SerializeField] AsteroidSpawnerSettings _spawnerSettings;
        [SerializeField] Transform _asteroidsTemplateParent;
        [SerializeField] SIAsteroidBehaviour[] _spawnedAsteroids;

        float _cameraZ;
        Camera _mainCamera;

        public SIAsteroidBehaviour[] SpawnedAsteroids => _spawnedAsteroids;

        void Start()
        {
            TryToLoadSetup();
            Initialise();
            TryToSpawn();
        }

        void TryToLoadSetup()
        {
            if (_spawnerSetup == null)
            {            
                Debug.LogError("No asteroid's spawner setup attached.", this);
                return;
            }
            _spawnerSettings = _spawnerSetup.asteroidSpawnerSettings;
        }
        void Initialise()
        {
            _asteroidsTemplateParent = transform;
            _mainCamera = SIGameMasterBehaviour.Instance.MainCamera;
            _cameraZ = _mainCamera.transform.localPosition.z;
            _spawnedAsteroids = new SIAsteroidBehaviour[_spawnerSettings.maxAsteroidsToSpawn];
        }
        Vector3 CalculateOutOfViewportPosition(int parentIndex)
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
            Vector3 viewportToWorldPosition = _mainCamera.ViewportToWorldPoint(viewportPosition);
            return viewportToWorldPosition;
        }

        void TryToSpawn()
        {
            for (int i = 0; i < _spawnerSettings.maxAsteroidsToSpawn; i++)
            {
                int spawnedPrefabIndex = Random.Range(0, _spawnerSettings.asteroidVariantsCount);
                int spawnedParentIndex = Random.Range(0, SIConstants.SCREEN_EDGES);

                AssignToParentAndCacheSpawnedObject(spawnedPrefabIndex, spawnedParentIndex, i);
            }
        }

        void AssignToParentAndCacheSpawnedObject(int spawnedPrefabIndex, int spawnedParentIndex, int i)
        {
            SIAsteroidBehaviour asteroidBehaviour =
                Instantiate(_spawnerSettings.asteroidVariants[spawnedPrefabIndex], _asteroidsTemplateParent);

            Transform asteroidTransform = asteroidBehaviour.transform;
            asteroidTransform.localPosition =
                asteroidTransform.InverseTransformPoint(CalculateOutOfViewportPosition(spawnedParentIndex));
            asteroidBehaviour.gameObject.SetActive(true);
            _spawnedAsteroids[i] = asteroidBehaviour;
        }
    }
}