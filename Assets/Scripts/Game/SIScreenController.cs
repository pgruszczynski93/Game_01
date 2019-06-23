using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace SpaceInvaders
{
    public class SIScreenController : MonoBehaviour
    {
        // dodać IConfigurabe
        [Range(1f, 2f)][SerializeField] private float _percentageOffset;
        
        [SerializeField] private float _spawnHorizontalMinPosition;
        [SerializeField] private float _spawnHorizontalMaxPosition;
        [SerializeField] private float _spawnVerticalMinPosition;
        [SerializeField] private float _spawnVerticalMaxPosition;
        
        [SerializeField] private float _cameraOffsetZ;
        [SerializeField] private float _leftWorldEdge;
        [SerializeField] private float _rightWorldEdge;
        [SerializeField] private float _topWorldEdge;
        [SerializeField] private float _bottomWorldEdge;

        [SerializeField] private Camera _mainCamera;

        void Start()
        {
            Initialise();
        }

        void Initialise()
        {
            if (_mainCamera == null)
            {
                Debug.LogError("Assign camera first.");
                return;
            }

            CalculateWorldLimits();
        }

        private void CalculateWorldLimits()
        {
            _cameraOffsetZ = _mainCamera.transform.localPosition.z + _mainCamera.nearClipPlane;

            Vector3 viewportMaxDimensions = new Vector3(1, 1, _cameraOffsetZ);
            Vector3 worldMaxDimensions = _mainCamera.ViewportToWorldPoint(viewportMaxDimensions);

            _leftWorldEdge = worldMaxDimensions.x;
            _rightWorldEdge = -worldMaxDimensions.x;
            _topWorldEdge = -worldMaxDimensions.y;
            _bottomWorldEdge = worldMaxDimensions.y;

            _spawnHorizontalMaxPosition = _rightWorldEdge *  _percentageOffset;
            _spawnHorizontalMinPosition = _leftWorldEdge *  _percentageOffset;
            
            _spawnVerticalMaxPosition = _topWorldEdge *  _percentageOffset;
            _spawnVerticalMinPosition = _bottomWorldEdge *  _percentageOffset;
        }

        // Object's Far screen position is shifted by Camera Z position;
        public bool IsInTheScreenSpace(Vector3 objectPosition)
        {
            Vector3 objectShiftedPosition = new Vector3(
                objectPosition.x - _cameraOffsetZ,
                objectPosition.y - _cameraOffsetZ,
                objectPosition.z);

            return objectShiftedPosition.x >= _leftWorldEdge && objectShiftedPosition.x <= _rightWorldEdge &&
                   objectShiftedPosition.y >= _bottomWorldEdge && objectShiftedPosition.y <= _topWorldEdge;
        }
        
        public bool IsInTheSpawnSpace(Vector3 objectPosition)
        {// zdebugować te metode - dodać przeliczanie glebi obiektu wzgledem kamery 
            
//            float depthOffsetZ = _cameraOffsetZ - objectPosition.z;
            
            Vector3 objectShiftedPosition = new Vector3(
                objectPosition.x - _cameraOffsetZ,
                objectPosition.y - _cameraOffsetZ,
                objectPosition.z);

            return objectShiftedPosition.x >= _spawnHorizontalMinPosition && objectShiftedPosition.x <= _spawnHorizontalMaxPosition &&
                   objectShiftedPosition.y >= _spawnVerticalMinPosition && objectShiftedPosition.y <= _spawnVerticalMaxPosition;
        }
    }
}