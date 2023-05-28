using UnityEngine;

namespace PG.Game.Features.ObjectsPool {
    public interface IPoolable {
        void PerformOnPoolActions();
        void SetSpawnPosition(Vector3 spawnPos);
        void SetSpawnRotation(Vector3 spawnRot);
        void ManageScreenVisibility();
    }
}