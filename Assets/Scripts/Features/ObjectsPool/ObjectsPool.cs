using System.Collections.Generic;
using PG.Game.EventSystem;
using PG.Game.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PG.Game.Features.ObjectsPool {
    public abstract class ObjectsPool<T> : MonoBehaviour where T : MonoBehaviour {
        [SerializeField] protected int _poolCapacity;
        [SerializeField] protected T _prefabToSpawn;
        [SerializeField] protected List<T> _objectsPool;

        protected T _currentlyPooledObject;

        int _poolIndex;

        void Start() => Initialise();
        void OnEnable() => SubscribeEvents();
        void OnDisable() => UnsubscribeEvents();

        protected virtual void Initialise() {
            if (_objectsPool != null) {
                _currentlyPooledObject = _objectsPool[0];
            }
        }

        protected virtual void SubscribeEvents() {
            GeneralEvents.OnUpdate += HandleOnUpdate;
        }

        protected virtual void UnsubscribeEvents() {
            GeneralEvents.OnUpdate -= HandleOnUpdate;
        }

        void HandleOnUpdate() {
            CheckObjectsVisibility();
        }

        void CheckObjectsVisibility() {
            IPoolable currentObj;
            for (int i = 0; i < _objectsPool.Count; i++) {
                currentObj = (IPoolable)_objectsPool[i];
                if (currentObj == null) {
                    Debug.LogError("Poolable object is null!");
                    continue;
                }

                currentObj.ManageScreenVisibility();
            }
        }

        protected abstract void ManagePoolableObject();

        protected void SetNextObjectFromPool() {
            _currentlyPooledObject = _objectsPool[_poolIndex];
            ManagePoolableObject();
            ++_poolIndex;
            if (_poolIndex > _poolCapacity - 1)
                _poolIndex = 0;
        }

#if UNITY_EDITOR
        [Button]
        protected virtual void AssignPoolableObjects() {
            if (_objectsPool == null || _objectsPool.Count == 0) {
                Debug.Log("[SIObjectPool] No objects in pool - reload.");
                _objectsPool = new List<T>();
            }

            T obj;
            for (var i = 0; i < _objectsPool.Count; i++) {
                obj = _objectsPool[i];
                if (obj == null)
                    DestroyImmediate(obj);
                else
                    DestroyImmediate(_objectsPool[i]?.gameObject);
            }

            _objectsPool = new List<T>();
            T objectInstance;
            UnityEditor.Undo.RegisterFullObjectHierarchyUndo(gameObject, "Pool manager hierarchy changed");
            for (var i = 0; i < _poolCapacity; i++) {
                objectInstance = Instantiate(_prefabToSpawn, transform);
                UnityEditor.Undo.RegisterCreatedObjectUndo(objectInstance.gameObject, "Poolable object Instantiaton");
                objectInstance.transform.localPosition = ScreenUtils.HiddenObjectPosition;
                _objectsPool.Add(objectInstance);
            }
        }
#endif
    }
}