using UnityEngine;
using System;

namespace SpaceInvaders
{
    public static class SIHelpers
    {
        public static float CAMERA_MIN_VIEWPORT_X = 0.03125f;
        public static float CAMERA_MAX_VIEWPORT_X = 0.96875f;
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
        

        public static bool IsObjectInScreenVerticalBounds3D(this Vector3 objectViewportPos)
        {
            if(objectViewportPos.y >= CAMERA_MAX_VIEWPORT_Y || objectViewportPos.y <= CAMERA_MIN_VIEWPORT_Y)
            {
                return true;
            }

            return false;
        }

        public static Vector3 SnapToGrid(Vector3 pos, float offset)
        {
            float x = pos.x;
            float y = pos.y;
            float z = pos.z;
            x = SnapToGrid(x, offset);
            y = SnapToGrid(y, offset);
            z = SnapToGrid(z, offset);
            return new Vector3(x, y, z);
        }

        public static int SnapToGrid(int pos, int offset)
        {
            float x = pos;
            return Mathf.RoundToInt(x / offset) * offset;
        }

        public static float SnapToGrid(float pos, float offset)
        {
            float x = pos;
            return Mathf.Round(x / offset) * offset;
        }

    }
}
