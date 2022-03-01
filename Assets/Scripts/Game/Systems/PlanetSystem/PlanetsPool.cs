using Sirenix.OdinInspector;
using SpaceInvaders.ObjectsPool;

namespace SpaceInvaders.PlanetSystem {
    public class PlanetsPool : SIObjectPool<Planet> {
        
        protected override void ManagePooledObject() {
            
        }

        [Button]
        void RandomizePlanets() {
            for (int i = 0; i < _poolCapacity; i++) {
                _objectsPool[i].RandomizePlanetAndRings();
            }
        }
    }
}