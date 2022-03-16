using Sirenix.OdinInspector;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class PlanetsPool : SIObjectPool<Planet> {

        [SerializeField] BoxCollider _planetAreaCollider;

        float _areaDepth;
        Vector3 _planetStartPos;
        Vector3 _planetEndPos;
        Vector3 _areaCenter;
        Vector3 _areaExtents;
        
        Bounds _planetAreaBounds;

        protected override void Initialise() {
            base.Initialise();
            _planetAreaBounds = _planetAreaCollider.bounds;
            _areaCenter = _planetAreaBounds.center;
            _areaExtents = _planetAreaBounds.extents;
            _areaDepth = _planetAreaBounds.size.z;
            _planetStartPos = new Vector3(0 , _areaCenter.y + _areaExtents.y, _areaExtents.z);
            _planetEndPos = new Vector3(0, _areaCenter.y - _areaExtents.y,  _areaExtents.z);
        }

        protected override void ManagePooledObject() {
            _currentObjectFromPool.SetSpawnPosition(_planetStartPos);
            _currentObjectFromPool.RandomizePlanetAndRings();
            _currentObjectFromPool.UseObjectFromPool();
        }

        
        //wywyloac SetNextObjectFromPool na jakims ewencie, albo sprawic tak, zeby to sie działo przy zniknieciu planety
        [Button]
        void RandomizePlanets() {
            for (int i = 0; i < _poolCapacity; i++) {
                _objectsPool[i].RandomizePlanetAndRings();
            }
        }
    }
}