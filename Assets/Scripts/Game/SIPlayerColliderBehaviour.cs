﻿using UnityEngine;

namespace SpaceInvaders
{
    public class SIPlayerColliderBehaviour : SIMainColliderBehaviour<SIPlayerBehaviour>
    {

        [SerializeField] private bool _isVFX;

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
            if (_isVFX)
            {
                return;
            }

            SIHelpers.SISimpleLogger(this, "Player's got hit.", SimpleLoggerTypes.Log);
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
            
            if (bonus != null)
            {
                SIEventsHandler.BroadcastOnBonusCollision(bonus.BonusInfo);
            }
        }
    }
}