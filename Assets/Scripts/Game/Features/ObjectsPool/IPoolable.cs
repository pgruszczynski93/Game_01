using UnityEngine;

namespace SpaceInvaders.ObjectsPool {
    public interface IPoolable {
        void PerformOnPoolActions();
        void SetSpawnPosition(Vector3 spawnPos);
        void SetSpawnRotation(Vector3 spawnRot);
        void ManageScreenVisibility();
    }
}