using UnityEngine;

public static class TransformExtensions
{
    public static void SetLocalPositionX(this Transform transform, int x) => transform.SetLocalPositionX((float)x);
    public static void SetLocalPositionY(this Transform transform, int y) => transform.SetLocalPositionY((float)y);
    public static void SetLocalPositionZ(this Transform transform, int z) => transform.SetLocalPositionZ((float)z);
    public static void AddLocalPositionX(this Transform transform, int x) => transform.AddLocalPositionX((float)x);
    public static void AddLocalPositionY(this Transform transform, int y) => transform.AddLocalPositionY((float)y);
    public static void AddLocalPositionZ(this Transform transform, int x) => transform.AddLocalPositionZ((float)x);
    public static void MultiplyLocalPositionX(this Transform transform, int y) => transform.MultiplyLocalPositionX((float)y);
    public static void MultiplyLocalPositionY(this Transform transform, int z) => transform.MultiplyLocalPositionY((float)z);
    public static void MultiplyLocalPositionZ(this Transform transform, int x) => transform.MultiplyLocalPositionZ((float)x);
    public static void DivideLocalPositionX(this Transform transform, int x) => transform.DivideLocalPositionX((float)x);
    public static void DivideLocalPositionY(this Transform transform, int y) => transform.DivideLocalPositionY((float)y);
    public static void DivideLocalPositionZ(this Transform transform, int z) => transform.DivideLocalPositionZ((float)z);
    
    public static void SetLocalPositionX(this Transform transform, float x) => transform.localPosition = new(x, transform.localPosition.y, transform.localPosition.z);
    public static void SetLocalPositionY(this Transform transform, float y) => transform.localPosition = new(transform.localPosition.x, y, transform.localPosition.z);
    public static void SetLocalPositionZ(this Transform transform, float z) => transform.localPosition = new(transform.localPosition.x, transform.localPosition.y, z);
    public static void AddLocalPositionX(this Transform transform, float x) => transform.localPosition = new(transform.localPosition.x + x, transform.localPosition.y, transform.localPosition.z);
    public static void AddLocalPositionY(this Transform transform, float y) => transform.localPosition = new(transform.localPosition.x, transform.localPosition.y + y, transform.localPosition.z);
    public static void AddLocalPositionZ(this Transform transform, float z) => transform.localPosition = new(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + z);
    public static void MultiplyLocalPositionX(this Transform transform, float x) => transform.localPosition = new(transform.localPosition.x * x, transform.localPosition.y, transform.localPosition.z);
    public static void MultiplyLocalPositionY(this Transform transform, float y) => transform.localPosition = new(transform.localPosition.x, transform.localPosition.y * y, transform.localPosition.z);
    public static void MultiplyLocalPositionZ(this Transform transform, float z) => transform.localPosition = new(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z * z);
    public static void DivideLocalPositionX(this Transform transform, float x) => transform.localPosition = new(transform.localPosition.x / x, transform.localPosition.y, transform.localPosition.z);
    public static void DivideLocalPositionY(this Transform transform, float y) => transform.localPosition = new(transform.localPosition.x, transform.localPosition.y / y, transform.localPosition.z);
    public static void DivideLocalPositionZ(this Transform transform, float z) => transform.localPosition = new(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z / z);
    
    
    public static void SetGlobalPositionX(this Transform transform, int x) => transform.SetGlobalPositionX((float)x);
    public static void SetGlobalPositionY(this Transform transform, int y) => transform.SetGlobalPositionY((float)y);
    public static void SetGlobalPositionZ(this Transform transform, int z) => transform.SetGlobalPositionZ((float)z);
    public static void AddGlobalPositionX(this Transform transform, int x) => transform.AddGlobalPositionX((float)x);
    public static void AddGlobalPositionY(this Transform transform, int y) => transform.AddGlobalPositionY((float)y);
    public static void AddGlobalPositionZ(this Transform transform, int x) => transform.AddGlobalPositionZ((float)x);
    public static void MultiplyGlobalPositionX(this Transform transform, int y) => transform.MultiplyGlobalPositionX((float)y);
    public static void MultiplyGlobalPositionY(this Transform transform, int z) => transform.MultiplyGlobalPositionY((float)z);
    public static void MultiplyGlobalPositionZ(this Transform transform, int x) => transform.MultiplyGlobalPositionZ((float)x);
    public static void DivideGlobalPositionX(this Transform transform, int x) => transform.DivideGlobalPositionX((float)x);
    public static void DivideGlobalPositionY(this Transform transform, int y) => transform.DivideGlobalPositionY((float)y);
    public static void DivideGlobalPositionZ(this Transform transform, int z) => transform.DivideGlobalPositionZ((float)z);
    
    public static void SetGlobalPositionX(this Transform transform, float x) => transform.position = new(x, transform.position.y, transform.position.z);
    public static void SetGlobalPositionY(this Transform transform, float y) => transform.position = new(transform.position.x, y, transform.position.z);
    public static void SetGlobalPositionZ(this Transform transform, float z) => transform.position = new(transform.position.x, transform.position.y, z);
    public static void AddGlobalPositionX(this Transform transform, float x) => transform.position = new(transform.position.x + x, transform.position.y, transform.position.z);
    public static void AddGlobalPositionY(this Transform transform, float y) => transform.position = new(transform.position.x, transform.position.y + y, transform.position.z);
    public static void AddGlobalPositionZ(this Transform transform, float z) => transform.position = new(transform.position.x, transform.position.y, transform.position.z + z);
    public static void MultiplyGlobalPositionX(this Transform transform, float x) => transform.position = new(transform.position.x * x, transform.position.y, transform.position.z);
    public static void MultiplyGlobalPositionY(this Transform transform, float y) => transform.position = new(transform.position.x, transform.position.y * y, transform.position.z);
    public static void MultiplyGlobalPositionZ(this Transform transform, float z) => transform.position = new(transform.position.x, transform.position.y, transform.position.z * z);
    public static void DivideGlobalPositionX(this Transform transform, float x) => transform.position = new(transform.position.x / x, transform.position.y, transform.position.z);
    public static void DivideGlobalPositionY(this Transform transform, float y) => transform.position = new(transform.position.x, transform.position.y / y, transform.position.z);
    public static void DivideGlobalPositionZ(this Transform transform, float z) => transform.position = new(transform.position.x, transform.position.y, transform.position.z / z);

    public static void SetGlobalScale(this Transform transform, Vector3 vector)
    {
        if (!transform.parent)
        {
            transform.localScale = vector;
            return;
        }

        var parentO = transform.parent;
        transform.parent = null;
        transform.localScale = vector;
        transform.parent = parentO;
    }
}