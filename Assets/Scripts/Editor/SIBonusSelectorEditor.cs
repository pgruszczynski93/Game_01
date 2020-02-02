using SpaceInvaders;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace MindwalkerStudio.Tools
{
    [CustomEditor(typeof(SIBonusSelector))]
    public class SIBonusSelectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (!GUILayout.Button("Test bonus drop")) 
                return;
            
            SIBonusSelector bonusSelector = (SIBonusSelector) target;
            bonusSelector.DropBonus();
        }
    }
}
#endif
