using SpaceInvaders;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace MindwalkerStudio.Tools
{
    [CustomEditor(typeof(SIBonusSelectorSystem))]
    public class SIBonusSelectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (!GUILayout.Button("Test bonus drop")) 
                return;
            
            SIBonusSelectorSystem bonusSelectorSystem = (SIBonusSelectorSystem) target;
            bonusSelectorSystem.DropBonus();
        }
    }
}
#endif
