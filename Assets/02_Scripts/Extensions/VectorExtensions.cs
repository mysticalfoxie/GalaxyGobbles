using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 ToVector3(this Vector2 vector2, float z = 0) => new(vector2.x, vector2.y, z);
    
    public static Vector3 Multiply(this Vector3 vector1, float value) => vector1.Multiply(new Vector3(value, value, value));
    public static Vector3 Multiply(this Vector3 vector1, float x, float y, float z) => vector1.Multiply(new Vector3(x, y, z));
    public static Vector3 Multiply(this Vector3 vector1, int x, int y, int z) => vector1.Multiply(new Vector3(x, y, z));
    public static Vector3 Multiply(this Vector3 vector1, Vector3 vector2)
    {
        var x = vector1.x * vector2.x;
        var y = vector1.y * vector2.y;
        var z = vector1.z * vector2.z;
        return new Vector3(x, y, z);
    }
    
    public static Vector3 Subtract(this Vector3 vector1, float x, float y, float z) => vector1.Subtract(new Vector3(x, y, z));
    public static Vector3 Subtract(this Vector3 vector1, int x, int y, int z) => vector1.Subtract(new Vector3(x, y, z));
    public static Vector3 Subtract(this Vector3 vector1, Vector3 vector2) 
    {
        var x = vector1.x - vector2.x;
        var y = vector1.y - vector2.y;
        var z = vector1.z - vector2.z;
        return new Vector3(x, y, z);
    }
    
    public static Vector3 Add(this Vector3 vector1, float x, float y, float z) => vector1.Add(new Vector3(x, y, z));
    public static Vector3 Add(this Vector3 vector1, int x, int y, int z) => vector1.Add(new Vector3(x, y, z));
    public static Vector3 Add(this Vector3 vector1, Vector3 vector2)
    {
        var x = vector1.x + vector2.x;
        var y = vector1.y + vector2.y;
        var z = vector1.z + vector2.z;
        return new Vector3(x, y, z);
    }
    
    public static Vector3 Divide(this Vector3 vector1, float x, float y, float z) => vector1.Divide(new Vector3(x, y, z));
    public static Vector3 Divide(this Vector3 vector1, int x, int y, int z) => vector1.Divide(new Vector3(x, y, z));
    public static Vector3 Divide(this Vector3 vector1, Vector3 vector2)
    {
        var x = vector1.x / vector2.x;
        var y = vector1.y / vector2.y;
        var z = vector1.z / vector2.z;
        return new Vector3(x, y, z);
    }

    public static Vector3 SetX(this Vector3 vector, int x) => vector.SetX((float)x);
    public static Vector3 SetX(this Vector3 vector, float x) => new(x, vector.y, vector.z);

    public static Vector3 SetY(this Vector3 vector, int y) => vector.SetY((float)y);
    public static Vector3 SetY(this Vector3 vector, float y) => new(vector.x, y, vector.z);

    public static Vector3 SetZ(this Vector3 vector, int z) => vector.SetZ((float)z);
    public static Vector3 SetZ(this Vector3 vector, float z) => new(vector.x, vector.y, z);

    public static Vector3 AddX(this Vector3 vector, int x) => vector.AddX((float)x);
    public static Vector3 AddX(this Vector3 vector, float x) => new(vector.x + x, vector.y, vector.z);

    public static Vector3 AddY(this Vector3 vector, int y) => vector.AddY((float)y);
    public static Vector3 AddY(this Vector3 vector, float y) => new(vector.x, vector.y + y, vector.z);

    public static Vector3 AddZ(this Vector3 vector, int z) => vector.AddZ((float)z);
    public static Vector3 AddZ(this Vector3 vector, float z) => new(vector.x, vector.y, vector.z + z);

    public static Vector3 MultiplyX(this Vector3 vector, int x) => vector.MultiplyX((float)x);
    public static Vector3 MultiplyX(this Vector3 vector, float x) => new(vector.x * x, vector.y, vector.z);

    public static Vector3 MultiplyY(this Vector3 vector, int y) => vector.MultiplyY((float)y);
    public static Vector3 MultiplyY(this Vector3 vector, float y) => new(vector.x, vector.y * y, vector.z);

    public static Vector3 MultiplyZ(this Vector3 vector, int z) => vector.MultiplyZ((float)z);
    public static Vector3 MultiplyZ(this Vector3 vector, float z) => new(vector.x, vector.y, vector.z * z);

    public static Vector3 DivideX(this Vector3 vector, int x) => vector.DivideX((float)x);
    public static Vector3 DivideX(this Vector3 vector, float x) => new(vector.x / x, vector.y, vector.z);

    public static Vector3 DivideY(this Vector3 vector, int y) => vector.DivideY((float)y);
    public static Vector3 DivideY(this Vector3 vector, float y) => new(vector.x, vector.y / y, vector.z);

    public static Vector3 DivideZ(this Vector3 vector, int z) => vector.DivideZ((float)z);
    public static Vector3 DivideZ(this Vector3 vector, float z) => new(vector.x, vector.y, vector.z / z);

    public static Vector3 SubtractX(this Vector3 vector, int x) => vector.SubtractX((float)x);
    public static Vector3 SubtractX(this Vector3 vector, float x) => new(vector.x - x, vector.y, vector.z);

    public static Vector3 SubtractY(this Vector3 vector, int y) => vector.SubtractY((float)y);
    public static Vector3 SubtractY(this Vector3 vector, float y) => new(vector.x, vector.y - y, vector.z);

    public static Vector3 SubtractZ(this Vector3 vector, int z) => vector.SubtractZ((float)z);
    public static Vector3 SubtractZ(this Vector3 vector, float z) => new(vector.x, vector.y, vector.z - z);

    public static Vector3 Update(this Vector3 vector, int? x = null, int? y = null, int? z = null) => vector.Update((float?)x, y, z);
    public static Vector3 Update(this Vector3 vector, float? x = null, float? y = null, float? z = null) => new(x ?? vector.x, y ?? vector.y, z ?? vector.z);
}