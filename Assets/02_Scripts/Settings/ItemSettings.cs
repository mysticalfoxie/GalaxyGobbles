using System;
using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming

public class ItemSettings : ScriptableObject
{
    public const string SETTINGS_PATH = "Assets/10_Miscellaneous/03_Settings/CFG_Item Settings.asset";
    
    private static ItemSettings _data;
    public static ItemSettings Data => _data ??= GetOrCreateSettings();

    [Header("List of all Items")]
    [SerializeField]
    public ItemData[] Items;
    
    internal static ItemSettings GetOrCreateSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<ItemSettings>(SETTINGS_PATH);
        if (settings != null) return settings;
        
        settings = CreateDefaultSettings();
        AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
        AssetDatabase.SaveAssets();
        
        return settings;
    }

    private static ItemSettings CreateDefaultSettings()
    {
        var settings = CreateInstance<ItemSettings>();
        settings.Items = Array.Empty<ItemData>();
        return settings;
    }
}