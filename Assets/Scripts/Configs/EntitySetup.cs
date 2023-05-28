using UnityEngine;

namespace PG.Game.Configs {
    [CreateAssetMenu(fileName = "Entity config", menuName = "Configs/Entity")]
    public class EntitySetup : ScriptableObject {
        public float entityMaxHealth;
    }
}