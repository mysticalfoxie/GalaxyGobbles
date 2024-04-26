using System;
using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming

public class ItemSettings : ScriptableObject, ISettings
{
    public const string SETTINGS_PATH = "Assets/10_Miscellaneous/03_Settings/CFG_Item Settings.asset";
    
    private static ItemSettings _data;
    public static ItemSettings Data => _data ??= GetOrCreateSettings();

    [Header("List of all Items")]
    [SerializeField]
    public ItemData[] Items;
    
    internal static ItemSettings GetOrCreateSettings()
    {
#if UNITY_EDITOR
        var settings = AssetDatabase.LoadAssetAtPath<ItemSettings>(SETTINGS_PATH);
        if (settings != null) return settings;
        
        settings = CreateDefaultSettings();
        AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
        AssetDatabase.SaveAssets();
        
        return settings;
#else
        return References.Instance.ItemSettings;
#endif
    }

    private static ItemSettings CreateDefaultSettings()
    {
        var settings = CreateInstance<ItemSettings>();
        settings.Items = Array.Empty<ItemData>();
        return settings;
    }
}