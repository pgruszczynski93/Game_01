using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class PlanetMovement : MonoBehaviour, ICanMove, IModifyTimeSpeedMultiplier {
        
        Transform _thisTransform;
        
        void SubscribeEvents()
        {
            SIEventsHandler.OnUpdate += HandleOnUpdate;
        }

        void UnsubscribeEvents()
        {
            SIEventsHandler.OnUpdate -= HandleOnUpdate;
        }

        void HandleOnUpdate() {
            
        }
        
        public void MoveObject() {
            
        }

        public void StopObject() {
        }


        public void RequestTimeSpeedModification() {
        }

        public void SetTimeSpeedModifier(float timeSpeedModifier, float progress = 1) {
        }
    }
}