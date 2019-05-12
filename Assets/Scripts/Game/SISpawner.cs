using UnityEngine;

namespace SpaceInvaders
{
    public class SISpawner : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            //SIEventsHandler.OnWaveEnd += Respawn;
        }

        protected virtual void OnDisable()
        {
            //SIEventsHandler.OnWaveEnd += Respawn;
        }
    }

}
