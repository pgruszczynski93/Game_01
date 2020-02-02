using MindwalkerStudio.InspectorTools;
using UnityEngine;
using UnityScript.Lang;

namespace SpaceInvaders
{
    public class SIBonusSelector : MonoBehaviour
    {
        [SerializeField] SIBonusSelectorSetup _selectorSetup;
        [SerializeField] SIBonus[] _availableBonuses;

        float _currentDropChance;
        float[] _sortedDropChancesLookup;

        void Initialise()
        {
            FillBonusDropChances();
        }

        //poprawić to
        void FillBonusDropChances()
        {
            _sortedDropChancesLookup = new float[_selectorSetup.selectorSettings.Length];
            for (int i = 0; i < _sortedDropChancesLookup.Length; i++)
            {
                _sortedDropChancesLookup[i] = _selectorSetup.selectorSettings[i].dropChance;
            }
           
            System.Array.Sort(_sortedDropChancesLookup);

            for (var i = 0; i < _sortedDropChancesLookup.Length; i++)
            {
                Debug.Log("xd " + _sortedDropChancesLookup[i]);
            }
        }

        void Start()
        {
            Initialise();
        }

        void OnEnable()
        {
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        void AssignEvents()
        {
            //drop bonus when enemy dies
        }

        void RemoveEvents()
        {
            //drop bonus when enemy dies
        }

        void SelectBonus() { }

        // ten atrybut nie dziala, ale sobie dodałem bo czemu nie - jak zadziała to bedzie dzialac 
        [Button("Drop Bonus")]
        public void DropBonus()
        {
            Debug.Log("Droppiong");
            _currentDropChance = Random.Range(0f, 1f);
            FillBonusDropChances();

        }

        bool CanDropBonus()
        {
            for (int i = 0; i < _sortedDropChancesLookup.Length; i++)
            {
                
            }

            return false;
        }
    }
}