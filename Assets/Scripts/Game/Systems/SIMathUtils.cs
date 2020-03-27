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
    }
}