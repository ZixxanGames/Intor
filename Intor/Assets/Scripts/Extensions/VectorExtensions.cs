using UnityEngine;

namespace Scripts.Extensions
{
    public static class VectorExtensions
    {
        public static void Deconstruct(this Vector3 vector3, out float x, out float y, out float z) => (x, y, z) = (vector3.x, vector3.y, vector3.z);
        public static (float x, float y, float z) Deconstruct(this Vector3 vector3) => (vector3.x, vector3.y, vector3.z);

        public static Vector3 Wrap(this Vector3 vector3)
        {
            for (int i = 0; i < 3; i++)
            {
                vector3[i] %= 360;

                if (vector3[i] > 180) vector3[i] -= 360;
            }

            return vector3;
        }
    }
}