using UnityEngine;

namespace Extensions.Vector
{
    public static class VectorExtensions
    {
        public static float ZXDistance(this Vector3 v3, Vector3 other)
        {
            return Vector2.Distance(new Vector2(v3.x, v3.z), new Vector2(other.x, other.z));
        }
    }
}
