using System;
using UnityEngine;

namespace SpaceInvaders
{
    [Serializable]
    public struct LocalGridMinMax
    {
        public float localGridHorizontalMin;
        public float localGridHorizontalMax;
    }

    public class SIGridMovementLimiter : MonoBehaviour
    {
        [SerializeField] LocalGridMinMax minMaxPair;
        [SerializeField] SIEnemyBehaviour[] _enemies;

        bool _initialised;

        void OnEnable()
        {
            AssignEvents();
        }

        void OnDisable()
        {
            RemoveEvents();
        }

        void AssignEvents()
        {
            SIEventsHandler.OnWaveEnd += InitialReset;
        }

        void RemoveEvents()
        {
            SIEventsHandler.OnWaveEnd -= InitialReset;
        }

        void TryToUpdateGridDimensions()
        {
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

        void InitialReset()
        {
            minMaxPair.localGridHorizontalMax = SIConstants.GRID_RIGHT_EDGE;
            minMaxPair.localGridHorizontalMin = SIConstants.GRID_LEFT_EDGE;
        }

        void TryToResetGridMinMax()
        {
            // intentionally assigned min as max horizontal value and the same for max...
            minMaxPair.localGridHorizontalMax = SIConstants.GRID_LEFT_EDGE;
            minMaxPair.localGridHorizontalMin = SIConstants.GRID_RIGHT_EDGE;
        }

        void FindNewGridDimensions(float xLocalPos)
        {
            if (xLocalPos < minMaxPair.localGridHorizontalMin)
                minMaxPair.localGridHorizontalMin = xLocalPos;

            if (xLocalPos > minMaxPair.localGridHorizontalMax)
                minMaxPair.localGridHorizontalMax = xLocalPos;
        }

        public LocalGridMinMax CalculateGridMinMax()
        {
            if (!_initialised)
            {
                _initialised = true;

                minMaxPair = new LocalGridMinMax
                {
                    localGridHorizontalMax = SIConstants.GRID_RIGHT_EDGE,
                    localGridHorizontalMin = SIConstants.GRID_LEFT_EDGE
                };
            }

            TryToUpdateGridDimensions();
            return minMaxPair;
        }
    }
}