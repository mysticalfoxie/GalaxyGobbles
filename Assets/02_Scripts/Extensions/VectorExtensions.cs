using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 ToVector3(this Vector2 vector2, float z = 0) => new(vector2.x, vector2.y, z);
}