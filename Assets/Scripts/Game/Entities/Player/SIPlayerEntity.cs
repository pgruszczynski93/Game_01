using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerEntity : SIEntity
    {
        [SerializeField] EntitySetup _entitySetup;
        [SerializeField] EntitySettings _entitySettings;
        
        [SerializeField] SIStatistics _statistics;
        protected override void Initialise()
        {
            base.Initialise();
            _entitySettings = _entitySetup.entitySettings;
        }
    }
}