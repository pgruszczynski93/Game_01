using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "Grid shooting config", menuName = "Mindwalker Studio/Grid Shooting config")]

    public class GridShootingSetup : ScriptableObject
    {
        public GridShootingSettings shootingSettings;
    }
}