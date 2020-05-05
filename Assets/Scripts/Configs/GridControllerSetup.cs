using System;
using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "Grid controller config", menuName = "Mindwalker Studio/Grid controller config")]

    public class GridControllerSetup : ScriptableObject
    {
        public GridControllerSettings gridControllerSettings;
    }

    [Serializable]
    public struct GridControllerSettings
    {
        public int maxEnemiesInGridRow;
        public int maxEnemiesInGridColumn;
        public int maxEnemiesInGrid;
        public int maxNeighboursOfEnemyCount;
        public float newWaveCooldown;
        public float endWaveCooldown;
        public int[] enemiesLeftToUpdateGridMovementTier;

    }
}