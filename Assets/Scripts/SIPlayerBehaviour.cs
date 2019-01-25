using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerBehaviour : SIGenericSingleton<SIPlayerBehaviour>
    {
        [SerializeField] private SIPlayerMovement _playerMovement;

        public SIPlayerMovement PlayerMovemnt
        {
            get { return _playerMovement; }
        }

        public void UDpa()
        {

        }
    }
}
