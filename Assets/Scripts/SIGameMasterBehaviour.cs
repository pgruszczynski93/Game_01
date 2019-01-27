using UnityEngine;

namespace SpaceInvaders
{
    public class SIGameMasterBehaviour : SIGenericSingleton<SIGameMasterBehaviour>
    {
        [SerializeField] private Camera _mainCamera;

        public Camera MainCamera
        {
            get
            {
                if (_mainCamera == null)
                {
                    Debug.LogError("No camera assigned!");
                }
                return _mainCamera;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            SetInitialReferences();
        }

        private void SetInitialReferences()
        {

        }

        private void Update()
        {
            OnMovementUpdate();
        }

        private void OnMovementUpdate()
        {
            SIEventsHandler.OnObjectMovement?.Invoke();
        }
    }
}
