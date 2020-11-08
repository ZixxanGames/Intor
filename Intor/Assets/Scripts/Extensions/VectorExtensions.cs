using UnityEngine;

namespace Scripts.Extensions
{
    public static class VectorExtensions
    {
        public static void Deconstruct(this Vector3 vector3, out float x, out float y, out float z) => (x, y, z) = (vector3.x, vector3.y, vector3.z);
        public static void Deconstruct(this Vector2 vector2, out float x, out float y) => (x, y) = (vector2.x, vector2.y);

        public static (float x, float y, float z) Deconstruct(this Vector3 vector3) => (vector3.x, vector3.y, vector3.z);
        public static (float x, float y) Deconstruct(this Vector2 vector2) => (vector2.x, vector2.y);

        public static Vector3 Wrap(this Vector3 vector)
        {
            for (int i = 0; i < 3; i++)
            {
                vector[i] %= 360;

                if (vector[i] > 180) vector[i] -= 360;
            }

            return vector;
        }

        public static Vector3 RandomizeInRange(this Vector3 vector, Vector3 maxRange, Vector3 minRange = default)
        {
            for (int i = 0; i < 3; i++)
            {
                var min = minRange[i];
                var max = maxRange[i];

                vector[i] += Random.Range(min, max);
            }

            return vector;
        }
        public static Vector3 RandomizeInRange(Vector3 maxRange, Vector3 minRange = default)
        {
            var vector = new Vector3();

            for (int i = 0; i < 3; i++)
            {
                var min = minRange[i];
                var max = maxRange[i];

                vector[i] += Random.Range(min, max);
            }

            return vector;
        }

        public static Vector3 RandomizeInSphere(this Vector3 vector, float radius, bool allowRandomY = false)
        {
            var y = vector.y;

            vector += Random.onUnitSphere * Random.Range(0, radius);

            if (!allowRandomY) vector.y = y;

            return vector;
        }
    }
}