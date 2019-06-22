using UnityEngine;

namespace SpaceInvaders
{
    public class SIGameConfiguratorBehaviour : MonoBehaviour
    {
        [SerializeField] private ScriptableSettingsMaster _scriptableSettingsMaster;
        [SerializeField] private MonoBehaviour[] _configurableObjects;

        private int _configurablesCount;

        private void Awake()
        {
            Initialise();
        }

        private void Initialise()
        {
            _configurablesCount = _configurableObjects.Length;
            
            if (_scriptableSettingsMaster == null || _configurableObjects == null || _configurablesCount == 0)
            {
                Debug.LogError("Assign proper properties first.", this);
                return;
            }
            InjectSettings();
        }

        private void InjectSettings()
        {
            IConfigurableObject configurableObject;

            for (int i = 0; i < _configurablesCount; i++)
            {
                configurableObject = (IConfigurableObject) _configurableObjects[i];
                configurableObject.Configure(_scriptableSettingsMaster);
            }
        }
    }
}