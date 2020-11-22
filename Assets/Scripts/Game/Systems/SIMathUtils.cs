using UnityEngine;

namespace SpaceInvaders
{
    public static class SIMathUtils
    {
        public const float COMPARSION_TOLERANCE = 1e-5f;

        public static bool IsEqualTo(float source, float target)
        {
            return Mathf.Abs(source - target) < COMPARSION_TOLERANCE;
        }

//        public static bool IsEqualToTarget(this float source, float target)
//        {
//            return Mathf.Abs(source - target) < COMPARSION_TOLERANCE;
//        }

        public static float Remap(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
        {
            return (oldValue - oldMin) / (oldMax - oldMin) * (newMax - newMin) + newMin;
        }

        public static float Remap01(float oldValue, float oldMin, float oldMax)
        {
            return (oldValue - oldMin) / (oldMax - oldMin);
        }
    }
}