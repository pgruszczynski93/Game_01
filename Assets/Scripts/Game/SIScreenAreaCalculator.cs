using UnityEngine;

namespace SpaceInvaders
{
    [System.Serializable]
    public struct ScreenEdges
    {
        public float leftScreenEdge;
        public float rightScreenEdge;
        public float topScreenEdge;
        public float bottomScreenEdge;
    }
    public class SIScreenAreaCalculator : MonoBehaviour
    {
        [SerializeField] private float _cameraOffsetZ;

        private Camera _mainCamera;
        private ScreenEdges _screenEdges;

        public ScreenEdges CalculateWorldLimits()
        {
            _mainCamera = SIGameMasterBehaviour.Instance.MainCamera;
            
            _cameraOffsetZ = _mainCamera.transform.localPosition.z + _mainCamera.nearClipPlane;

            Vector3 viewportMaxDimensions = new Vector3(1, 1, _cameraOffsetZ);
            Vector3 worldMaxDimensions = _mainCamera.ViewportToWorldPoint(viewportMaxDimensions);

            _screenEdges = new ScreenEdges
            {
                leftScreenEdge = Mathf.Round(worldMaxDimensions.x),
                rightScreenEdge = -Mathf.Round(worldMaxDimensions.x),
                topScreenEdge = -Mathf.Round(worldMaxDimensions.y),
                bottomScreenEdge = Mathf.Round(worldMaxDimensions.y)
            };

            return _screenEdges;
        }
    }
}