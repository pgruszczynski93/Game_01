using UnityEngine;

namespace SpaceInvaders
{
    public class SIEnemyMovement : SIMovement, IMoveable
    {
        protected override void SetInitialReferences()
        {
            base.SetInitialReferences();

            BASIC_SPEED = 10f;
        }

        protected override void OnEnable()
        {
            SIEventsHandler.OnObjectMovement += MoveObj;
        }

        protected override void OnDisable()
        {
            SIEventsHandler.OnObjectMovement -= MoveObj;
        }

        public void MoveObj()
        {
            MoveObject(2, true);
        }
    }
}
