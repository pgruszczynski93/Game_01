using UnityEngine;

namespace SpaceInvaders
{
    public static class SIScreenUtils {
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
        public static bool IsInHorizontalWorldScreenLimit(Vector3 currentPosition, float leftLimit, float rightLimit)
        {
            float horizontalPosition = currentPosition.x;
            return horizontalPosition >= leftLimit && horizontalPosition <= rightLimit;
        }
        public static bool IsInVerticalWorldScreenLimit(Vector3 currentPosition, float bottomLimit, float topLimit)
        {
            float verticalPosition = currentPosition.y;
            return verticalPosition >= bottomLimit && verticalPosition <= topLimit;
        }
    }
}