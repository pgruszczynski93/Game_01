using UnityEngine;

namespace PG.Game.Configs {
    [CreateAssetMenu(fileName = "New Bonus", menuName = "Configs/Bonus")]
    public class ScriptableBonus : ScriptableObject {
        public BonusSettings bonusSettings;
    }
}