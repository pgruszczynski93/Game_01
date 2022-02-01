using UnityEngine;

namespace SpaceInvaders.PlanetSystem {
    public class PlanetMovement : MonoBehaviour, IModifyTimeSpeedMultiplier {
        
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
        
        //dodać interfejs/ manager, który ogarnie wszystkie obiekty, które są widoczne na ekranie, a przesuwane w dół
        // tak zeby mialy wspolne metody

        void CheckIsProjectileOnScreen()
        {
            Vector3 bonusViewPortPosition =
                SIGameMasterBehaviour.Instance.MainCamera.WorldToViewportPoint(_thisTransform.position);

            if (!bonusViewPortPosition.IsInVerticalViewportSpace())
                StopObject();
        }
        
        public void StopObject() {
            // TryEnableBonusAndSelectedVariant(false);
            // ResetMotion();
        }


        public void RequestTimeSpeedModification() {
            throw new System.NotImplementedException();
        }

        public void SetTimeSpeedModifier(float timeSpeedModifier, float progress = 1) {
            throw new System.NotImplementedException();
        }
    }
}