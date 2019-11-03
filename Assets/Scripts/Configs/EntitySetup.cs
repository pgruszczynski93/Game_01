using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "Entity config", menuName = "Mindwalker Studio/Entity config")]
    public class EntitySetup : ScriptableObject
    {
        public EntitySettings entitySettings;
    }
}