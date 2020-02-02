using UnityEngine;

namespace SpaceInvaders
{
    [CreateAssetMenu(fileName = "Bonus Selector Settings Config", menuName = "Mindwalker Studio/Bonus Selector Settings Config")]
    public class SIBonusSelectorSettings : ScriptableObject
    {
        [Range(0f, 1f)] public float dropChance;
    }
}