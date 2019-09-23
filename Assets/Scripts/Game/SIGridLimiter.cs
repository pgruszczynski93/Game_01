using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    public class SIGridLimiter : MonoBehaviour
    {
        [SerializeField] private float _localGridHorizontalMin;
        [SerializeField] private float _localGridHorizontalMax;
        [SerializeField] private SIEnemyBehaviour[] _enemies;

        private bool _initialised;
        private void OnEnable()
        {
            AssignEvents();
        }

        private void OnDisable()
        {
            RemoveEvents();
        }

        void AssignEvents()
        {
            SIEventsHandler.OnWaveEnd += ResetGridMinMax;
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnWaveEnd -= ResetGridMinMax;
        }

        private void TryToUpdateGridDimensions()
        {
            Debug.Log("[SIGridLimiter] Grid recalculates.");
            float xLocalPos;
            Transform enemyTransform;
            SIEnemyBehaviour enemy;
            ResetGridMinMax();
            for (int i = 0; i < _enemies.Length; i++)
            {
                enemy = _enemies[i];
                if(enemy.IsEnemyAlive() == false)
                    continue;


                enemyTransform = enemy.transform;
                xLocalPos = enemyTransform.localPosition.x;
                FindNewGridDimensions(xLocalPos);
            }
        }

        private void ResetGridMinMax()
        {
            // intentionally assigned min as max horizontal value and the same for max...
            _localGridHorizontalMax = SIConstants.GRID_LEFT_EDGE;
            _localGridHorizontalMin = SIConstants.GRID_RIGHT_EDGE;
        }

        private void FindNewGridDimensions(float xLocalPos)
        {
            if (xLocalPos < _localGridHorizontalMin)
            {
                _localGridHorizontalMin = xLocalPos;
            }

            else if (xLocalPos > _localGridHorizontalMax)
            {
                _localGridHorizontalMax = xLocalPos;
            }
        }

        public float HalfGridLength()
        {
            if (_initialised == false)
            {
                _initialised = true;
                ResetGridMinMax();
                return Mathf.Abs(_localGridHorizontalMax - _localGridHorizontalMin) * 0.5f;
            }
            TryToUpdateGridDimensions();
            return Mathf.Abs(_localGridHorizontalMax - _localGridHorizontalMin) * 0.5f;
        }
    }
}