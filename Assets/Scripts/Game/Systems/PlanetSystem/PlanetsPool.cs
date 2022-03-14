using Sirenix.OdinInspector;
using SpaceInvaders.ObjectsPool;
using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class PlanetsPool : SIObjectPool<Planet> {
        
        public Vector3 protoStartPos;
        public Vector3 protoFinishPos;

        protected override void ManagePooledObject() {
            _currentObjectFromPool.SetSpawnPosition(protoStartPos);
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