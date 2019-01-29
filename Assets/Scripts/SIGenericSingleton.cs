using UnityEngine;

namespace SpaceInvaders
{
    public class SIGenericSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] existingInstances = FindObjectsOfType<T>();
                    int existingInstancesCount = existingInstances.Length;

                    if (existingInstancesCount > 1)
                    {
                        Debug.Log("MANY " + typeof(T));
                        Debug.LogError("To many instances of " + typeof(T) + " objects ");
                    }

                    if (existingInstancesCount == 1)
                    {
                        _instance = existingInstances[0];
                        Debug.Log("ONE " + typeof(T) +" " + _instance.GetInstanceID() + " " + _instance.name);
                    }

                    if (existingInstancesCount == 0)
                    {
                        Debug.Log("ZERO " + typeof(T));

                        GameObject singleInstance = new GameObject();
                        singleInstance.name = typeof(T).Name;
                        _instance = singleInstance.AddComponent<T>();

                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Debug.Log("DESTROY " + typeof(T) +" "+ gameObject.GetInstanceID());

                DestroyImmediate(gameObject);
                return;
            }
            Debug.Log("LET LIVE " + typeof(T) + " " + gameObject.GetInstanceID());
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}
