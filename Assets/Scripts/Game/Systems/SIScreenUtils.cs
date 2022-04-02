using UnityEngine;

namespace SpaceInvaders
{
    public static class SIScreenUtils {
        public const float VIEWPORT_SPAWN_MIN = -0.25f;
        public const float VIEWPORT_SPAWN_MAX = 1.25f;
        
        public static Vector3 HiddenObjectPosition = new Vector3(0, 0, -100);
        // public static Vector3 HiddenObjectPosition = new Vector3(0, 0, 0);
        public static bool IsInCameraFrustum(Renderer renderer, Camera cam)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
            return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }
        
        public static bool IsInVerticalViewportSpace(this Vector3 objectViewportPos)
        {
            return objectViewportPos.y >= 0.0f && objectViewportPos.y <= 1.0f;
        }
        
        public static bool IsInHorizontalViewportSpace(this Vector3 objectViewportPos)
        {
            return objectViewportPos.x >= 0.0f && objectViewportPos.x <= 1.0f;
        }
        
        public static bool IsInHorizontalWorldScreenLimit(float horizontalPos, float leftOffset = 0f, float rightOffset = 0f)
        {
            ScreenEdges screenWorldEdges = SIGameMasterBehaviour.Instance.ScreenAreaCalculator.CalculatedScreenEdges;
            float leftLimit = screenWorldEdges.leftScreenEdge + leftOffset;
            float rightLimit = screenWorldEdges.rightScreenEdge + rightOffset;
            return horizontalPos >= leftLimit && horizontalPos <= rightLimit;
        }
        public static bool IsInVerticalWorldScreenLimit(float verticalPos, float topOffset = 0f, float bottomOffset = 0f)
        {
            ScreenEdges screenWorldEdges = SIGameMasterBehaviour.Instance.ScreenAreaCalculator.CalculatedScreenEdges;
            float topLimit = screenWorldEdges.topScreenEdge + topOffset;
            float bottomLimit = screenWorldEdges.bottomScreenEdge + bottomOffset;
            return verticalPos >= bottomLimit && verticalPos <= topLimit;
        }

        public static bool IsHigherThanVerticalScreenLimit(float verticalPos, float offset = 0f) {
            ScreenEdges screenWorldEdges = SIGameMasterBehaviour.Instance.ScreenAreaCalculator.CalculatedScreenEdges;
            float topLimit = screenWorldEdges.topScreenEdge + offset;
            return verticalPos > topLimit;
        }

        public static bool IsLowerThanVerticalScreenLimit(float verticalPos, float offset = 0f) {
            ScreenEdges screenWorldEdges = SIGameMasterBehaviour.Instance.ScreenAreaCalculator.CalculatedScreenEdges;
            float bottomLimit = screenWorldEdges.bottomScreenEdge + offset;
            return verticalPos < bottomLimit;
        }
    }
}