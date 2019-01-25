using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerBehaviour : SIGenericSingleton<SIPlayerBehaviour>
    {
        [SerializeField] private SIPlayerMovement _playerMovement;
        [SerializeField] private SIPlayerShootBehaviour _playerShoot;

        public SIPlayerMovement PlayerMovemnt
        {
            get { return _playerMovement; }
        }

        public SIPlayerShootBehaviour PlayerShoot
        {
            get { return _playerShoot; }
        }
    }
}
