using System;
using UnityEngine;

namespace PG.Game.Systems {
    [Serializable]
    public struct ScreenEdges {
        public float leftScreenEdge;
        public float rightScreenEdge;
        public float topScreenEdge;
        public float bottomScreenEdge;
    }

    public class ScreenAreaCalculator : MonoBehaviour {
        [SerializeField] float _cameraOffsetZ;

        Camera _mainCamera;
        ScreenEdges _screenEdges;

        void Awake() {
            CalculateWorldLimits();
        }

        public ScreenEdges CalculatedScreenEdges => _screenEdges;

        void CalculateWorldLimits() {
            _mainCamera = GameMasterBehaviour.Instance.MainCamera;

            _cameraOffsetZ = _mainCamera.transform.localPosition.z + _mainCamera.nearClipPlane;

            Vector3 viewportMaxDimensions = new Vector3(1, 1, _cameraOffsetZ);
            Vector3 worldMaxDimensions = _mainCamera.ViewportToWorldPoint(viewportMaxDimensions);

            _screenEdges = new ScreenEdges {
                leftScreenEdge = Mathf.Round(worldMaxDimensions.x),
                rightScreenEdge = -Mathf.Round(worldMaxDimensions.x),
                topScreenEdge = -Mathf.Round(worldMaxDimensions.y),
                bottomScreenEdge = Mathf.Round(worldMaxDimensions.y)
            };
        }
    }
}