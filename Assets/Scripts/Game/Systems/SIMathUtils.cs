using UnityEngine;

namespace SpaceInvaders
{
    public static class SIMathUtils
    {
        public const float COMPARSION_TOLERANCE = 1e-5f;
        public const float FULL_EULER_ANGLE = 360f;

        public static bool IsApproximatelyEqualTo(float source, float target)
        {
            return Mathf.Abs(source - target) < COMPARSION_TOLERANCE;
        }
        
        public static float Remap(float oldValue, float oldMin, float oldMax, float newMin, float newMax)
        {
            return Remap01(oldValue, oldMin, oldMax) * (newMax - newMin) + newMin;
        }

        public static float Remap01(float oldValue, float oldMin, float oldMax)
        {
            return (oldValue - oldMin) / (oldMax - oldMin);
        }

        public static Color GetRandomColorRGB() {
            return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
        
        public static Color GetRandomColorRGBA(bool useAlpha, float alpha = 0f) {
            Color rgbCol = GetRandomColorRGB();
            return new Color(rgbCol.r, rgbCol.g, rgbCol.b, !useAlpha ? 0 : alpha);
        }

        public static Vector3 GetRandomEulerAngle() {
            return new Vector3(Random.Range(0f, 360f), 
                Random.Range(0f, 360f), 
                Random.Range(0f, 360f));
        }
    }
}