using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "Player Movement Config", menuName = "Mindwalker Studio/Player Movement Config")]
    public class PlayerMovementSetup : MovementSetup
    {
        public PlayerMovementSettings playerMovementSettings;
    }
}