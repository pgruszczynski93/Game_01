using UnityEngine;

namespace PG.Game.Configs {
    [CreateAssetMenu(fileName = "Player Movement Config", menuName = "Configs/Player Movement")]
    public class PlayerMovementSetup : MovementSetup {
        public PlayerMovementSettings playerMovementSettings;
    }
}