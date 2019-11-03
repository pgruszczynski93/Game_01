using UnityEngine;

namespace SpaceInvaders
{
    public class SIEntity : MonoBehaviour
    {
        [SerializeField] EntitySetup _entitySetup;
        [SerializeField] EntitySettings _entitySettings;
        
        [SerializeField] SIStatistics _statistics;

        bool _initialised;

        public SIStatistics Statistics => _statistics;

        protected virtual void Initialise()
        {
            if (_initialised)
                return;

            _initialised = true;
            _entitySettings = _entitySetup.entitySettings;
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