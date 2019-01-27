using UnityEngine;

namespace SpaceInvaders
{
    public class SIGameMasterBehaviour : SIGenericSingleton<SIGameMasterBehaviour>
    {
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
