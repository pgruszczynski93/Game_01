using System;

namespace MindwalkerStudio.InspectorTools
{
    // dokończyć 
    [AttributeUsage(AttributeTargets.Method)]
    public class Button : Attribute
    {
        string _buttonText;

        public Button(string buttonText)
        {
            _buttonText = buttonText;
        }
    }
}