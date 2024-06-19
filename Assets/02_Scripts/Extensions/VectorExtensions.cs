using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 ToVector3(this Vector2 vector2, float z = 0) => new(vector2.x, vector2.y, z);

    public static Vector3 Clone(this Vector3 vector3) => new(vector3.x, vector3.y, vector3.z);
}