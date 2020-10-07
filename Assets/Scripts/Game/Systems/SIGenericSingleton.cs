using UnityEngine;

namespace SpaceInvaders
{
    public class SIGenericSingleton<T> : MonoBehaviour where T : Component
    {
         static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) 
                    return _instance;
                
                T[] existingInstances = FindObjectsOfType<T>();
                int existingInstancesCount = existingInstances.Length;

                if (existingInstancesCount > 1)
                {
                    Debug.LogError("To many instances of " + typeof(T) + " objects ");
                }

                switch (existingInstancesCount) {
                    case 1:
                        _instance = existingInstances[0];
                        break;
                    case 0: {
                        GameObject singleInstance = new GameObject();
                        singleInstance.name = typeof(T).Name;
                        _instance = singleInstance.AddComponent<T>();
                        break;
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}
