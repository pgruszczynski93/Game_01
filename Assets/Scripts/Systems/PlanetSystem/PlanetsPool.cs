using System;
using Cysharp.Threading.Tasks;
using PG.Game.EventSystem;
using PG.Game.Features.ObjectsPool;
using PG.Game.Helpers;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PG.Game.Systems.PlanetSystem {
    public class PlanetsPool : ObjectsPool<Planet> {
        [SerializeField] bool _canManagePool;
        [SerializeField] float _maxAxisInclination;
        [SerializeField] float _minRespawnInterval;
        [SerializeField] float _maxRespawnInterval;
        [SerializeField] BoxCollider _planetAreaCollider;

        bool _isStopped;

        //todo: dorobić zatrzymywanie polowania w momencie, kiedy dostepna jest fala asteroid
        //todo: najlepiej zrobi to tak, że fala asteroid bedzie mogla zostac wylosowan w momencie, kiedy zadnej planety nie ma widocznej na ekranie
        Bounds _currentObjectBounds;
        Bounds _planetAreaBounds;

#if UNITY_EDITOR
        void OnDrawGizmosSelected() {
            _planetAreaBounds = _planetAreaCollider.bounds;

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(_planetAreaBounds.min, 5);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_planetAreaBounds.max, 5);
        }
#endif

        protected override void Initialise() {
            base.Initialise();
            PlanetPoolingTask().Forget();
        }

        protected override void SubscribeEvents() {
            base.SubscribeEvents();
            GeneralEvents.OnGameStateChanged += HandleOnGameStateChanged;
        }

        protected override void UnsubscribeEvents() {
            base.UnsubscribeEvents();
            GeneralEvents.OnGameStateChanged -= HandleOnGameStateChanged;
        }

        void HandleOnGameStateChanged(GameStates gameState) {
            if (gameState == GameStates.GameStarted) {
                EnablePoolManagement();
            }
        }

        void EnablePoolManagement() {
            _canManagePool = true;
        }

        async UniTaskVoid PlanetPoolingTask() {
            while (true) {
                if (_currentlyPooledObject != null) {
                    await UniTask.WaitUntil(() => !_currentlyPooledObject.IsMoving());
                    if (_canManagePool) {
                        float interval = Random.Range(_minRespawnInterval, _maxRespawnInterval);
                        await UniTask.Delay(TimeSpan.FromSeconds(interval));
                        ManagePoolableObject();
                    }
                }

                await UniTask.Yield();
            }
        }

        Vector3 CalculateSpawnPosition() {
            _planetAreaBounds = _planetAreaCollider.bounds;
            Vector3 areaCenter = _planetAreaBounds.center;
            Vector3 areaExtents = _planetAreaBounds.extents;

            float areaWidthSize = _planetAreaBounds.min.x + _planetAreaBounds.size.x;
            float areaDepthSize = _planetAreaBounds.min.z + _planetAreaBounds.size.z;
            float areaMinX = _planetAreaBounds.min.x;
            float areaMinZ = _planetAreaBounds.min.z;
            float y = areaCenter.y + areaExtents.y;

            Bounds currPlanetBounds = _currentlyPooledObject.GetPlanetBounds();
            float planetExtentsX = currPlanetBounds.extents.x;
            float planetExtentsZ = currPlanetBounds.extents.z;

            float spawnMinX = areaMinX + planetExtentsX;
            float spawnMinZ = areaMinZ + planetExtentsZ;
            float spawnMaxX = areaWidthSize - planetExtentsX;
            float spawnMaxZ = areaDepthSize - planetExtentsZ;

            float randX = Random.Range(spawnMinX, spawnMaxX);
            float randZ = Random.Range(spawnMinZ, spawnMaxZ);

            return new Vector3(randX, y, randZ);
        }

        Vector3 GetPlanetInclinationXZ() {
            float xAxisInclination = Random.Range(-_maxAxisInclination, _maxAxisInclination);
            float zAxisInclination = Random.Range(-_maxAxisInclination, _maxAxisInclination);
            return new Vector3(xAxisInclination, 0, zAxisInclination);
        }

        protected override void ManagePoolableObject() {
            _currentlyPooledObject.RandomizePlanetAndRings();
            CalculateSpawnPosition();
            _currentlyPooledObject.SetSpawnPosition(CalculateSpawnPosition());
            _currentlyPooledObject.SetSpawnRotation(GetPlanetInclinationXZ());
            _currentlyPooledObject.PerformOnPoolActions();
        }

        [Button]
        void RandomizePlanets() {
            for (int i = 0; i < _poolCapacity; i++) {
                _objectsPool[i].RandomizePlanetAndRings();
            }
        }

        [Button]
        void AlignPlanetTest() {
            ManagePoolableObject();
        }
    }
}