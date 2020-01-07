using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "Bonus config", menuName = "Mindwalker Studio/Bonus config")]
    public class BonusSetup : ScriptableObject
    {
        public BonusSettings bonusSettings;
    }
}