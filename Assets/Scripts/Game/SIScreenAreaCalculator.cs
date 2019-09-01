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
        [SerializeField] private ScreenEdges _screenEdges;

        private bool initialised;
        private Camera _mainCamera;

        public ScreenEdges AllScreenEdges => _screenEdges;

        void Start()
        {
            Initialise();
        }

        void Initialise()
        {
            if (initialised)
                return;

            initialised = true;
            _mainCamera = SIGameMasterBehaviour.Instance.MainCamera;
            CalculateWorldLimits();
        }

        private void CalculateWorldLimits()
        {
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
        }
    }
}