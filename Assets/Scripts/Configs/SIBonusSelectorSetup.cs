using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "Bonus Selector Setup Config", menuName = "Mindwalker Studio/Bonus Selector Setup Config")]

    public class SIBonusSelectorSetup : ScriptableObject
    {
        public SIBonusSelectorSettings[] selectorSettings;
    }
}