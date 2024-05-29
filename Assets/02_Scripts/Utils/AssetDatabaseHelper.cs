using System.Linq;
using UnityEditor;

public static class AssetDatabaseHelper
{
    public static (string AssetPath, T Value)[] LoadAssetsOfType<T>() where T : UnityEngine.Object
    {
        return AssetDatabase
            .FindAssets($"t:{typeof(T).Name}")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(x => (Asset: x, Value: AssetDatabase.LoadAssetAtPath<T>(x)))
            .ToArray();
    }
}