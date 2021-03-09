using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "New Bonus", menuName = "Mindwalker Studio/New Bonus")]
    public class BonusSetup : ScriptableObject
    {
        public BonusSettings bonusSettings;
    }
}