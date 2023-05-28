using System;
using UnityEngine;

namespace PG.Game.Configs {
    [CreateAssetMenu(fileName = "Grid controller config", menuName = "Configs/Grid controller")]
    public class GridControllerSetup : ScriptableObject {
        public GridControllerSettings gridControllerSettings;
    }

    [Serializable]
    public struct GridControllerSettings {
        public int maxEnemiesInGridRow;
        public int maxEnemiesInGridColumn;
        public int maxEnemiesInGrid;
        public int maxNeighboursOfEnemyCount;
    }
}