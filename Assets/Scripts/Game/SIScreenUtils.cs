using UnityEngine;

namespace SpaceInvaders
{
    public static class SIScreenUtils
    {
        public static bool IsInCameraFrustum(Renderer renderer, Camera cam)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
            return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }
        
        
    }
}