using UnityEngine;

namespace SpaceInvaders
{
    public class SIEntity : MonoBehaviour
    {
        bool _initialised;

        protected virtual void Initialise()
        {
            if (_initialised)
                return;

            _initialised = true;
        }

        void Start()
        {
            Initialise();
        }

        protected virtual void OnEnable()
        {
            AssignEvents();
        }

        protected virtual void OnDisable()
        {
            RemoveEvents();
        }

        protected virtual void AssignEvents() { }

        protected virtual void RemoveEvents() { }
    }
}