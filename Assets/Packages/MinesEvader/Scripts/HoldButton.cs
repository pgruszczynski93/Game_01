using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace MinesEvader
{

    /// <summary>
    /// We need this script because Unity's UI button script does not give us the hold
    /// effect we need to keep playing the moving methods we created for the player
    /// when building for mobile.
    /// The OnPointerDown/Up will be called when the button is pressed/unpressed.
    /// </summary>
    public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        // This event is used to call the player moving methods.
        [Tooltip("Methods will get called every frame when the button is on hold")]
        public UnityEvent OnHold; 

        [Tooltip("Reference for the the button sprite in normal mode.")]
        public RectTransform Unpressed; 

        [Tooltip("Reference for the the button sprite in hold mode.")]
        public RectTransform Pressed;

        bool holding = false;


        void Awake()
        {
            Unpressed.gameObject.SetActive(true);
            Pressed.gameObject.SetActive(false);
        }

        void Update()
        {
            if (holding) OnHold.Invoke();
        }

        public void OnPointerDown(PointerEventData data)
        {          
            holding = true;

            Unpressed.gameObject.SetActive(false);
            Pressed.gameObject.SetActive(true);
        }

        public void OnPointerUp(PointerEventData data)
        {            
            holding = false;

            Unpressed.gameObject.SetActive(true);
            Pressed.gameObject.SetActive(false);
        }

    }

}