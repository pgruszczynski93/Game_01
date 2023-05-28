using System;
using PG.Game.Systems.WaveSystem;
using PG.Game.EventSystem;
using PG.Game.Helpers;
using UnityEngine;

namespace PG.Game.Entities.Enemies {
    [Serializable]
    public struct LocalGridMinMax {
        public float localGridHorizontalMin;
        public float localGridHorizontalMax;
    }

    public class GridMovementLimiter : MonoBehaviour {
        [SerializeField] LocalGridMinMax minMaxPair;
        [SerializeField] EnemyBehaviour[] _enemies;

        bool _initialised;

        void OnEnable() {
            AssignEvents();
        }

        void OnDisable() {
            RemoveEvents();
        }

        void AssignEvents() {
            GameplayEvents.OnWaveEnd += HandleWaveEnd;
        }

        void RemoveEvents() {
            GameplayEvents.OnWaveEnd -= HandleWaveEnd;
        }

        void TryToUpdateGridDimensions() {
            float xLocalPos;
            Transform enemyTransform;
            EnemyBehaviour enemy;
            TryToResetGridMinMax();
            for (int i = 0; i < _enemies.Length; i++) {
                enemy = _enemies[i];
                if (!enemy.IsEnemyAlive())
                    continue;


                enemyTransform = enemy.transform;
                xLocalPos = enemyTransform.localPosition.x;
                FindNewGridLocalMinMax(xLocalPos);
            }
        }

        void HandleWaveEnd(WaveType waveType) {
            //todo: maybe apply to other wave types - no if check
            SetStartLimits();
        }

        void SetStartLimits() {
            minMaxPair.localGridHorizontalMax = Consts.GRID_RIGHT_EDGE;
            minMaxPair.localGridHorizontalMin = Consts.GRID_LEFT_EDGE;
        }

        void TryToResetGridMinMax() {
            // intentionally assigned min as max horizontal value and the same for max...
            minMaxPair.localGridHorizontalMax = Consts.GRID_LEFT_EDGE;
            minMaxPair.localGridHorizontalMin = Consts.GRID_RIGHT_EDGE;
        }

        void FindNewGridLocalMinMax(float xLocalPos) {
            if (xLocalPos < minMaxPair.localGridHorizontalMin)
                minMaxPair.localGridHorizontalMin = xLocalPos;

            if (xLocalPos > minMaxPair.localGridHorizontalMax)
                minMaxPair.localGridHorizontalMax = xLocalPos;
        }

        public LocalGridMinMax CalculateGridMinMax() {
            if (!_initialised) {
                _initialised = true;

                minMaxPair = new LocalGridMinMax {
                    localGridHorizontalMax = Consts.GRID_RIGHT_EDGE,
                    localGridHorizontalMin = Consts.GRID_LEFT_EDGE
                };
            }

            TryToUpdateGridDimensions();
            return minMaxPair;
        }
    }
}