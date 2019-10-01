using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders
{
    [System.Serializable]
    public struct LocalGridMinMax
    {
        public float localGridHorizontalMin;
        public float localGridHorizontalMax;
    }

    public class SIGridLimiter : MonoBehaviour
    {
        private const float MIN_MAX_TOLERANCE = 1e-05f;
        [SerializeField] private LocalGridMinMax minMaxPair;
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
            SIEventsHandler.OnWaveEnd += TryToResetGridMinMax;
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnWaveEnd -= TryToResetGridMinMax;
        }

        private void TryToUpdateGridDimensions()
        {
            Debug.Log("[SIGridLimiter] Grid recalculates.");
            float xLocalPos;
            Transform enemyTransform;
            SIEnemyBehaviour enemy;
            TryToResetGridMinMax();
            for (int i = 0; i < _enemies.Length; i++)
            {
                enemy = _enemies[i];
                if (enemy.IsEnemyAlive() == false)
                    continue;


                enemyTransform = enemy.transform;
                xLocalPos = enemyTransform.localPosition.x;
                FindNewGridDimensions(xLocalPos);
            }
        }

        private void TryToResetGridMinMax()
        {
            // intentionally assigned min as max horizontal value and the same for max...
            minMaxPair.localGridHorizontalMax = 2*SIConstants.GRID_LEFT_EDGE;
            minMaxPair.localGridHorizontalMin = 2*SIConstants.GRID_RIGHT_EDGE;
        }

        private void FindNewGridDimensions(float xLocalPos)
        {
            if (xLocalPos < minMaxPair.localGridHorizontalMin)
                minMaxPair.localGridHorizontalMin = xLocalPos;

            if (xLocalPos > minMaxPair.localGridHorizontalMax)
                minMaxPair.localGridHorizontalMax = xLocalPos;
        }

        public LocalGridMinMax CalculateGridMinMax()
        {
            if (_initialised == false)
            {
                _initialised = true;

                minMaxPair = new LocalGridMinMax
                {
                    localGridHorizontalMax = SIConstants.GRID_RIGHT_EDGE,
                    localGridHorizontalMin = SIConstants.GRID_LEFT_EDGE,
                };
            }

            TryToUpdateGridDimensions();
            return minMaxPair;
        }
    }
}