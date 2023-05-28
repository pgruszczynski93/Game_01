using UnityEngine;

namespace PG.Game.Configs {
    [CreateAssetMenu(fileName = "Grid Movement Config", menuName = "Configs/Grid Movement")]
    public class GridMovementSetup : MovementSetup {
        public GridMovementSettings gridMovementSettings;
    }
}