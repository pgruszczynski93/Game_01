using UnityEngine;

namespace SpaceInvaders
{
    public class SISpawner : SIGenericSingleton<SISpawner>, IRespawnable
    {

        protected virtual void OnEnable()
        {
            //SIEventsHandler.OnWaveEnd += Respawn;
        }

        protected virtual void OnDisable()
        {
            //SIEventsHandler.OnWaveEnd += Respawn;
        }

        public void Respawn()
        {
        }
    }

}
