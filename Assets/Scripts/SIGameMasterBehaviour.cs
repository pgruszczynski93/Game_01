using UnityEngine;

namespace SpaceInvaders
{
    public class SIGameMasterBehaviour : SIGenericSingleton<SIGameMasterBehaviour>
    {
        [SerializeField] private Camera _mainCamera;

        protected override void Awake()
        {
            base.Awake();

            SetInitialReferences();
        }

        private void SetInitialReferences()
        {

        }
    }
}
