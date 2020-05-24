using SpaceInvaders;
using UnityEditor;

namespace MindwalkerStudio.Tools
{
    // todo: IMPORT ODIN
    [CustomPropertyDrawer(typeof(SIDroppedBonuses))]
    public class SIDroppedBonusesDrawer : DictionaryDrawer<BonusType, SIBonus> { }
}