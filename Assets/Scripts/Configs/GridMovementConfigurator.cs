using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "Grid Movement Config", menuName = "Mindwalker Studio/Grid Movement Config")]
    public class GridMovementConfigurator : ScriptableObject
    {
        public GridMovementSettings gridMovementSettings;
    }
}