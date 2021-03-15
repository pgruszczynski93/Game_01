using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "New Bonus", menuName = "Mindwalker Studio/New Bonus")]
    public class ScriptableBonus : ScriptableObject
    {
        public BonusSettings bonusSettings;
    }
}