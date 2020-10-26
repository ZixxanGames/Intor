using UnityEngine;

namespace Scripts.Extensions
{
    public static class QuaternionExtensions
    {
        public static void Deconstruct(this Quaternion quaternion, out float x, out float y, out float z, out float w) => (x, y, z, w) = (quaternion.x, quaternion.y, quaternion.z, quaternion.w);

        public static (float x, float y, float z, float w) Deconstruct(this Quaternion quaternion) => (quaternion.x, quaternion.y, quaternion.z, quaternion.w);
    }
}


