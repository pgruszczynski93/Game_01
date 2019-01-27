using UnityEngine;
using System;

namespace SpaceInvaders
{
    public static class SIHelpers
    {
        public static float CAMERA_MIN_VIEWPORT_X = 0.05f;
        public static float CAMERA_MAX_VIEWPORT_X = 0.95f;
        public static float CAMERA_MIN_VIEWPORT_Y = 0.0f;
        public static float CAMERA_MAX_VIEWPORT_Y = 1.0f;

        public static bool IsObjectInScreenHorizontalBounds2D(this Vector2 objectViewportPos)
        {
            if (objectViewportPos.x >= CAMERA_MAX_VIEWPORT_X || objectViewportPos.x <= CAMERA_MIN_VIEWPORT_X)
            {
                return true;
            }

            return false;
        }
        

        public static bool IsObjectInScreenVerticalBounds(this Vector3 objectViewportPos)
        {
            if(objectViewportPos.y >= CAMERA_MAX_VIEWPORT_Y || objectViewportPos.y <= CAMERA_MIN_VIEWPORT_Y)
            {
                return true;
            }

            return false;
        }

    }
}
