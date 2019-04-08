using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders
{
    public class SIBonusSlotBehaviour : MonoBehaviour
    {
        [SerializeField] private bool _canDisplayLevelGraphics;
        [SerializeField] private string _bonusTypeText;
        [SerializeField] private Text _slotText;
        [SerializeField] private GameObject _slotLevelGraphics;

        private void Start()
        {
            SetInitialReferences();
        }

        private void SetInitialReferences()
        {
            SetBonusText();
            TryToEnableLevelGraphics();
        }

        private void SetBonusText()
        {
            if (string.IsNullOrEmpty(_bonusTypeText))
            {
                SIHelpers.SISimpleLogger(this, SIConstantsHelpers.EMPTY_STRING_ERROR, SimpleLoggerTypes.Error);
                return;
            }
            _slotText.text = _bonusTypeText;
        }

        private void TryToEnableLevelGraphics()
        {
            if (_slotLevelGraphics == null)
            {
                SIHelpers.SISimpleLogger(this, SIConstantsHelpers.NOT_ASSIGNED_REFERENCE_ERROR, SimpleLoggerTypes.Error);
                return;
            }

            _slotLevelGraphics.SetActive(_canDisplayLevelGraphics);
        }

        public void EnableBonusSlot(bool canBeEnabled)
        {
            gameObject.SetActive(canBeEnabled);
        }
    }

}
