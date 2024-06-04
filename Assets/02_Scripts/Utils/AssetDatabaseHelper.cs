// ReSharper disable once RedundantUsingDirective - Required in pre-processor
using System;
using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;

public static class AssetDatabaseHelper
{
    public static (string AssetPath, T Value)[] LoadAssetsOfType<T>() where T : Object
    {
#if UNITY_EDITOR
        return AssetDatabase
            .FindAssets($"t:{typeof(T).Name}")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(x => (Asset: x, Value: AssetDatabase.LoadAssetAtPath<T>(x)))
            .ToArray();
#else
        return Array.Empty<(string AssetPath, T Value)>();
#endif
    }
}