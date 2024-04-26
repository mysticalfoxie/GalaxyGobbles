using System;
using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming

public class LevelSettings : ScriptableObject
{
    public const string SETTINGS_PATH = "Assets/10_Miscellaneous/03_Settings/CFG_Level Settings.asset";
    
    private static LevelSettings _data;
    public static LevelSettings Data => _data ??= GetOrCreateSettings();

    [Header("List of all Levels")]
    [SerializeField]
    public LevelData[] Levels;

    
    internal static LevelSettings GetOrCreateSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<LevelSettings>(SETTINGS_PATH);
        if (settings != null) return settings;
        
        settings = CreateDefaultSettings();
        AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
        AssetDatabase.SaveAssets();
        
        return settings;
    }

    private static LevelSettings CreateDefaultSettings()
    {
        var settings = CreateInstance<LevelSettings>();
        settings.Levels = Array.Empty<LevelData>();
        return settings;
    }
}