using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.ObjectsPool {
    public abstract class SIObjectPool<T> : MonoBehaviour where T : MonoBehaviour{

        [SerializeField] protected int _poolCapacity;
        [SerializeField] protected T _prefabToSpawn;
        [SerializeField] protected List<T> _objectsPool;
        
        protected int _poolIndex;

        protected abstract void ManagePooledObject();
        
        protected void UpdatePool() {
            ManagePooledObject();
            ++_poolIndex;
            if (_poolIndex > _poolCapacity - 1)
                _poolIndex = 0;
        }
        
        //todo: make this pool instantiate any object selected
     
// #if UNITY_EDITOR
//         [Button]
//         void AssignBonuses() {
//             for (var i = 0; i < _objectsPool.Count; i++) {
//                 DestroyImmediate(_objectsPool[i]?.gameObject);
//             }
//
//             _objectsPool = new List<SIBonus>();
//             SIBonus bonusInstance;
//             UnityEditor.Undo.RegisterFullObjectHierarchyUndo(gameObject, "Bonus hierarchy changed");
//             for (var i = 0; i < _poolCapacity; i++) {
//                 bonusInstance = Instantiate(_prefabToSpawn, transform);
//                 UnityEditor.Undo.RegisterCreatedObjectUndo(bonusInstance.gameObject, "Bonus Instantiaton");
//                 bonusInstance.Parent = transform;
//                 bonusInstance.transform.localPosition = SIScreenUtils.HiddenObjectPosition;
//                 _objectsPool.Add(bonusInstance);
//             }
//         }
// #endif
    }
}