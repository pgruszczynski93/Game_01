using Sirenix.OdinInspector;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class PlanetsPool : SIObjectPool<Planet> {

        [SerializeField] BoxCollider _planetAreaCollider;

        float _areaWidthSize;
        float _areaHeightSize;
        float _areaDepthSize;

        Vector3 _planetStartPos;
        Vector3 _planetEndPos;
        Vector3 _areaCenter;
        Vector3 _areaExtents;

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
            SetRefs();
        }

        void SetRefs() {
            _planetAreaBounds = _planetAreaCollider.bounds;
            _areaCenter = _planetAreaBounds.center;
            _areaExtents = _planetAreaBounds.extents;
            _areaWidthSize = _planetAreaBounds.min.x + _planetAreaBounds.size.x;
            _areaHeightSize = _planetAreaBounds.min.y + _planetAreaBounds.size.y;
            _areaDepthSize = _planetAreaBounds.min.z + _planetAreaBounds.size.z;
            _planetStartPos = new Vector3(0, _areaCenter.y + _areaExtents.y, _areaExtents.z);
            _planetEndPos = new Vector3(0, _areaCenter.y - _areaExtents.y, _areaExtents.z);
        }

        protected override void ManagePooledObject() {
            SetTopAreaSurfacePosition();
            _currentObjectFromPool.SetSpawnPosition(_planetStartPos);
            _currentObjectFromPool.RandomizePlanetAndRings();
            _currentObjectFromPool.UseObjectFromPool();
        }

        void SetTopAreaSurfacePosition() {
            float areaMinX = _planetAreaBounds.min.x;
            float areaMinZ = _planetAreaBounds.min.z;
            float y = _planetStartPos.y;

            Bounds currPlanetBounds = _currentObjectFromPool.GetPlanetBounds();
            float planetExtentsX = currPlanetBounds.extents.x;
            float planetExtentsZ = currPlanetBounds.extents.z;

            float spawnMinX = areaMinX + planetExtentsX;
            float spawnMinZ = areaMinZ + planetExtentsZ;
            float spawnMaxX = _areaWidthSize - planetExtentsX;
            float spawnMaxZ = _areaDepthSize - planetExtentsZ;
            
            float randX = Random.Range(spawnMinX, spawnMaxX);
            float randZ = Random.Range(spawnMinZ, spawnMaxZ);
            
            _planetStartPos = new Vector3(randX, y, randZ);
        }
        
        //wywyloac SetNextObjectFromPool na jakims ewencie, albo sprawic tak, zeby to sie działo przy zniknieciu planety
        [Button]
        void RandomizePlanets() {
            for (int i = 0; i < _poolCapacity; i++) {
                _objectsPool[i].RandomizePlanetAndRings();
            }
        }
        
        //TEST PUPROSE
        [Button]
        void TestPlanetAlignment() {
            SetRefs();
            SetNextObjectFromPool();
            ManagePooledObject();
        }
    }
}