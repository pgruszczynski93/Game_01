using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerColliderBehaviour : SIMainColliderBehaviour<SIPlayerBehaviour>
    {
        protected override void OnEnable()
        {
            //onCollisionCallback += OnPlayerHitted;
            _onCollisionActions["Enemy"] += OnPlayerHitted;
            _onCollisionActions["EnemyProjectile"] += OnPlayerHitted;
            _onCollisionActions["Bonus"] += OnBonusGained;
        }

        protected override void OnDisable()
        {
            //onCollisionCallback -= OnPlayerHitted;
            _onCollisionActions["Enemy"] -= OnPlayerHitted;
            _onCollisionActions["EnemyProjectile"] -= OnPlayerHitted;
            _onCollisionActions["Bonus"] -= OnBonusGained;
        }


        private void OnPlayerHitted(MonoBehaviour collisionBehaviour = null)
        {
            SIHelpers.SISimpleLogger(this, "Player's got hit.", SimpleLoggerTypes.Log);
            SIEventsHandler.OnPlayerHit?.Invoke();
        }

        private void OnBonusGained(MonoBehaviour collisionBehaviour = null)
        {
            if (collisionBehaviour == null)
            {
                SIHelpers.SISimpleLogger(this, "Can't assign bonuses to the player.", SimpleLoggerTypes.Error);
                return;
            }

            SIHelpers.SISimpleLogger(this, "Player got bonus.", SimpleLoggerTypes.Log);

            SIBonus bonus = collisionBehaviour as SIBonus;
            SIEventsHandler.OnBonusCollision(bonus.BonusInfo);
        }
    }
}
